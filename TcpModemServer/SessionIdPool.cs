using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Framework.Utilities.Tracing;

namespace TcpModemServer
{
    public interface ISessionIdPool
    {
        int Acquire();
        void Release(int id);
        void Close();
    }

    [Export(typeof(ISessionIdPool))]
    public class SessionIdPool : ISessionIdPool
    {
        private int _next = 1;
        private readonly Dictionary<int, bool> _sessionIdPool = Enumerable.Range(1, 32).ToDictionary(p => p, p => false); 
        public int Acquire()
        {
            lock (_sessionIdPool)
            {
                int id = 0;
                var count = _sessionIdPool.Count;
                for (int i = _next; i < _next + count; i++)
                {
                    var key = GetKey(i, count);
                    if (!_sessionIdPool[key])
                    {
                        id = key;
                        _sessionIdPool[key] = true;
                        _next = GetKey(id + 1,count);
                        break;
                    }
                }

                return id;
            }
        }

        private static int GetKey(int i, int count)
        {
            var key = i % (count+1);
            return key == 0 ? 1 : key;
        }

        public void Release(int id)
        {
            lock (_sessionIdPool)
            {
                if (!_sessionIdPool.ContainsKey(id))
                {
                    TraceUtil.Warn("Invalid session id to release: {0}", id);
                }

                _sessionIdPool[id] = false;
            }
        }

        public void Close()
        {
            lock (_sessionIdPool)
            {
                foreach (var sessionId in _sessionIdPool.Keys.ToList())
                {
                    _sessionIdPool[sessionId] = false;
                }

                _next = 1;
            }
        }
    }
}
