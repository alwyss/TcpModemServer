using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Framework.Utilities.Helpers;
using TcpModemServer.CommandHandling;

namespace TcpModemServer
{
    public interface ICommandRegistry
    {
        ICommandHandler GetCommand(string command);
    }

    [Export(typeof(ICommandRegistry))]
    public class CommandRegistry : ICommandRegistry
    {
        private Dictionary<string, ICommandHandler> _handlers;

        [ImportingConstructor]
        public CommandRegistry([ImportMany]IEnumerable<ICommandHandler> handlers)
        {
            _handlers = handlers.ToDictionary(p => p.CommandText, p => p);
        }

        public ICommandHandler GetCommand(string command)
        {
            return _handlers.ValueOrDefault(command);
        }
    }
}
