namespace TcpModemServer.CommandHandling
{
    public interface ICommandHandler
    {
        string CommandText { get; }
        string Response { get; set; }
        bool HasData { get; }

        bool HandleData(string request, ref int start);
        bool HandleCommand(CommandInfo commandInfo);
    }
}