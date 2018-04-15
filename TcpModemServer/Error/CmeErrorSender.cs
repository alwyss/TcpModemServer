using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpModemServer.Error
{
    public interface ICmeErrorSender
    {
        void SendCmeError(CmeErrorCode error);
    }

    [Export(typeof(ICmeErrorSender))]
    class CmeErrorSender : ICmeErrorSender
    {
        private readonly IResponseSender _responseSender;

        [ImportingConstructor]
        public CmeErrorSender(IResponseSender responseSender)
        {
            _responseSender = responseSender;
        }

        public void SendCmeError(CmeErrorCode error)
        {
            _responseSender.SendResponse("+CME_ERROR "+ (int)error);
        }
    }
}
