using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Framework.Utilities.Helpers;
using TcpModemServer.Sessions;

namespace TcpModemServer
{
    [Export(typeof(ISessionManager))]
    public class SessionManager : ISessionManager
    {
        private readonly ISessionIdPool _pool;
        private readonly Dictionary<int, IDataSession> _sessions = new Dictionary<int, IDataSession>();
        private readonly IEventBus _eventBus;
        private readonly INotifSender _notifSender;
        private readonly ISocketDataReceiver _socketDataReceiver;
        private Action _modemChannelClosed;

        [ImportingConstructor]
        public SessionManager(ISessionIdPool pool, IEventBus eventBus, INotifSender notifSender,
            ISocketDataReceiver socketDataReceiver)
        {
            _pool = pool;
            _eventBus = eventBus;
            _notifSender = notifSender;
            _socketDataReceiver = socketDataReceiver;
            _modemChannelClosed = Close;
            _eventBus.Add(EventKeys.ChannelClosed, _modemChannelClosed);
        }

        private void OnDteChannelClosed()
        {
            
        }

        public IDataSession Add(DataProtocol protocol)
        {
            lock (_sessions)
            {
                var id = _pool.Acquire();
                if (id != 0)
                {
                    var session = CreateSession(id, protocol);
                    _sessions[id] = session;
                    return session;
                }

                return null;
            }
        }

        private IDataSession CreateSession(int id, DataProtocol protocol)
        {
            if(protocol == DataProtocol.TCP)
                return new TcpSession(id, _eventBus, _notifSender, _socketDataReceiver);
            if(protocol == DataProtocol.UDP)
                return new UdpSession(id, _eventBus, _notifSender, _socketDataReceiver);

            throw new NotSupportedException("Protocol not supported: " + protocol);
        }

        public IDataSession GetSession(int sessionId)
        {
            lock (_sessions)
            {
                return _sessions.ValueOrDefault(sessionId);
            }
        }

        public void Remove(int sessionId)
        {
            lock (_sessions)
            {
                _sessions.Remove(sessionId);
                _pool.Release(sessionId);
            }
        }

        public void Close()
        {
            //_eventBus.Remove(EventKeys.ChannelClosed, _modemChannelClosed);
            lock (_sessions)
            {
                foreach (var session in _sessions)
                {
                    session.Value.Close();
                    _pool.Release(session.Key);
                }

                _sessions.Clear();

                _pool.Close();
            }
        }

        public TcpSession GetSession()
        {
            throw new NotImplementedException();
        }
    }
}
