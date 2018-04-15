using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace TcpModemServer
{
    [Export(typeof(ICommandTypeHelper))]
    public class CommandTypeHelper : ICommandTypeHelper
    {
        private readonly Dictionary<string, CommandType> _commandTypes = new Dictionary<string, CommandType>()
        {
            {"=?", CommandType.Test },
            {"?", CommandType.Read },
            {"=", CommandType.Write },
            {"\r", CommandType.Execute }
        };

        public CommandInfo GetCommandInfo(string commandLine)
        {
            var start = 0;
            foreach (var command in _commandTypes)
            {
                var pos = commandLine.IndexOf(command.Key, start);
                if (pos != -1)
                {
                    var commandInfo = new CommandInfo(commandLine.Substring(start, pos - start).Trim(), command.Value,
                        commandLine, pos + command.Key.Length);
                    return commandInfo;
                }
            }

            return null;
        }
    }
}
