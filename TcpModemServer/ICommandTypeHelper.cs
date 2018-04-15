namespace TcpModemServer
{
    public interface ICommandTypeHelper
    {
        CommandInfo GetCommandInfo(string commandLine);
    }
}