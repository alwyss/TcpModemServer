using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    public interface ICommandParser
    {
        bool Parse(string request, ref int start, out ICommandHandler commandHandler);
        void Cleanup();
    }

    [Export(typeof(ICommandParser))]
    public class CommandParser : ICommandParser
    {
        private string _commandLine;
        private readonly ICommandTypeHelper _commandTypeHelper;
        private readonly ICommandRegistry _commandRegistry;

        [ImportingConstructor]
        public CommandParser(ICommandTypeHelper commandTypeHelper, ICommandRegistry commandRegistry)
        {
            _commandTypeHelper = commandTypeHelper;
            _commandRegistry = commandRegistry;
            Cleanup();
        }

        /// <returns>True if command processing is done; false otherwise.</returns>
        public bool Parse(string request, ref int start, out ICommandHandler commandHandler)
        {
            commandHandler = null;
            var pos = request.IndexOf("\r", start);
            if (pos == -1)
            {
                //_state = CommandParseState.WaitForCommand;
                _commandLine = request;
                start = request.Length;
                return false;
            }

            try
            {
                var commandLine = _commandLine + request.Substring(start, pos - start);
                start = pos + 1;

                var commandInfo = _commandTypeHelper.GetCommandInfo(commandLine);
                if (commandInfo == null) return true;

                commandHandler = _commandRegistry.GetCommand(commandInfo.CommandName);

                if (commandHandler == null)
                {
                    return true;
                }

                return commandHandler.HandleCommand(commandInfo);
            }
            finally
            {
                Cleanup();
            }
        }

        public void Cleanup()
        {
            _commandLine = "";
        }
    }
}
