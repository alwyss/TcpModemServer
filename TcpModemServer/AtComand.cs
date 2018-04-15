using System;
using System.ComponentModel.Composition;
using Framework.Utilities.Helpers;
using TcpModemServer.CommandHandling;
using TcpModemServer.Error;

namespace TcpModemServer
{
    public abstract class AtComand : ICommandHandler
    {
        public abstract string CommandText { get; }

        public int SessionId { get; private set; }

        public object[] Arguments { get; private set; }
        public string Response { get; set; }

        [Import]
        protected IResponseSender ResponseSender { get; private set; }

        [Import]
        protected INotifSender NotifSender { get; private set; }

        [Import]
        protected ISessionManager SessionManager { get; private set; }

        [Import]
        protected ICmeErrorSender CmeErrorSender { get; private set; }

        protected abstract bool Handle(CommandInfo commandInfo);

        /// <returns>True if command processing is done; false otherwise.</returns>
        public bool HandleCommand(CommandInfo commandInfo)
        {
            if(Handle(commandInfo)) return !HasData;

            return true;
        }

        public virtual bool HasData { get { return false; } }

        public bool HandleData(string request, ref int start)
        {
            if (HasData) return OnHandleData(request, ref start);
            return true;
        }

        protected virtual bool OnHandleData(string request, ref int pos)
        {
            return true;
        }

        protected void SendResponse(string response)
        {
            ResponseSender.SendResponse(response);
        }

        protected static bool ParseItem<TItem>(string command, ref int start, out TItem item,
            TItem defaultValue = default(TItem))
        {
            var pos = command.IndexOf(",", start);
            if (pos == -1)
            {
                pos = command.Length;
            }

            var result = ParserHelper.TryParse(command.Substring(start, pos - start), out item);
            if (pos < command.Length) start = pos + 1;
            return result;
        }

        protected void SendTcpNotif(int sessionId, TcpNotifCode tcpNotif)
        {
            if (tcpNotif != TcpNotifCode.OK)
                NotifSender.SendTcpNotif(sessionId, tcpNotif);
        }

        protected void SendNotif(int sessionId, int notifCode, DataProtocol protocol)
        {
            NotifSender.SendNotif(sessionId, notifCode, protocol);
        }

        protected string Unescape(string remoteAddress)
        {
            var trimmedAddr = remoteAddress.Trim();
            if (trimmedAddr == string.Empty) return trimmedAddr;
            var start = trimmedAddr.IndexOf("\"", 0, 1);
            var end = trimmedAddr.LastIndexOf("\"", remoteAddress.Length-1, 1);
            if(start != -1 && end != -1 && end > start)
                return remoteAddress.Substring(start+1, end - start-1);

            if(start == -1 && end == -1) return trimmedAddr;

            throw new ArgumentException("Invalid remoteAddress: " + remoteAddress);
        }

        protected ITcpSession GetTcpSession(int sessionId)
        {
            var session = SessionManager.GetSession(sessionId);
            if (session == null)
            {
                NotifSender.SendTcpNotif(sessionId, TcpNotifCode.BadSessionId);
                return null;
            }

            return (ITcpSession)session;
        }

        protected void SendCmeError(CmeErrorCode cmeErrorCode)
        {
            CmeErrorSender.SendCmeError(cmeErrorCode);
        }

        protected IDataSession CreateSession(DataProtocol protocol)
        {
            var session = SessionManager.Add(protocol);
            if (session == null)
            {
                SendCmeError(CmeErrorCode.NoAvailableSession);
                return null;
            }

            return session;
        }

        protected IUdpSession GetUdpSession(int sessionId)
        {
            var session = SessionManager.GetSession(sessionId);
            if (session == null)
            {
                NotifSender.SendUdpNotif(sessionId, UdpNotifCode.BadSessionId);
                return null;
            }

            return (IUdpSession)session;
        }
    }
}
