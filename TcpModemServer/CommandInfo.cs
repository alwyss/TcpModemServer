namespace TcpModemServer
{
    public class CommandInfo
    {
        public CommandInfo(string commandName, CommandType commandType, string commandLine, int argsPos)
        {
            CommandName = commandName;
            CommandType = commandType;
            CommandLine = commandLine;
            ArgsPos = argsPos;
        }

        public string CommandName { get; private set; }
        public CommandType CommandType { get; private set; }
        public string CommandLine { get; private set; }
        public int ArgsPos { get; private set; }
    }
}