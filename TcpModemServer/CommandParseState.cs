namespace TcpModemServer
{
    public enum CommandParseState
    {
        Idle,
        WaitForCommand,
        Command,
        Data
    }
}