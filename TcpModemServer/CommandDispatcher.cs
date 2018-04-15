using System;
using System.ComponentModel.Composition;
using System.Text;
using Framework.Utilities.Helpers;
using Framework.Utilities.Tracing;
using TcpModemServer.CommandHandling;

namespace TcpModemServer
{
    public interface ICommandDispatcher : IDisposable
    {
        void Dispatch(byte[] data, int len);
    }

    [Export(typeof(ICommandDispatcher))]
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandRegistry _commandRegistry;
        private readonly ICommandTypeHelper _commandTypeHelper;
        private readonly IEventBus _eventBus;

        private CommandParseState _state=CommandParseState.Idle;
        private ICommandHandler _command;
        private readonly Action _channelClosedHandler;
        private readonly object _lock = new object();
        private readonly ICommandParser _commandParser;

        [ImportingConstructor]
        public CommandDispatcher(ICommandRegistry commandRegistry, ICommandTypeHelper commandTypeHelper,
            IResponseSender responseSender, IEventBus eventBus, ICommandParser commandParser)
        {
            _commandRegistry = commandRegistry;
            _commandTypeHelper = commandTypeHelper;
            this._eventBus = eventBus;
            _commandParser = commandParser;
            _channelClosedHandler = OnChannelClosed;
            _eventBus.Add(EventKeys.ChannelClosed, _channelClosedHandler);
        }

        private void OnChannelClosed()
        {
            FinishCommand();
        }

        public void Dispatch(byte[] data, int len)
        {
            lock (_lock)
            {
                var start = 0;
                var request = StringHelper.GetString(data, len);

                while (start < request.Length)
                {
                    if (_command != null)
                    {
                        HandleData(request, ref start);
                    }
                    else
                    {
                        ParseCommand(request, ref start);
                    }
                }
            }
        }

        private void ParseCommand(string request, ref int start)
        {
            //var pos = request.IndexOf("\r", start);
            //if (pos == -1)
            //{
            //    _state = CommandParseState.WaitForCommand;

            //    return;
            //}

            //var commandLine = request.Substring(start, pos-start);
            //start = pos + 1;

            //var commandInfo = _commandTypeHelper.GetCommandInfo(commandLine);
            //if (commandInfo == null) return;

            //_command = _commandRegistry.GetCommand(commandInfo.CommandName);
            //_state = CommandParseState.WaitForCommand;
            //_command = _commandParser.Parse(request, ref start);
            //if (_command == null) 
            //{
            //    //_state = CommandParseState.Idle;
            //    return;
            //}

            //_state = CommandParseState.Command;

            //var handData = _command.HandleCommand(commandInfo);

            //if(handData)
            //    _state = CommandParseState.Data;
            //else
            //    FinishCommand();

            try
            {
                ICommandHandler command;
                if (_commandParser.Parse(request, ref start, out command))
                {
                    FinishCommand();
                }
                else
                {
                    _command = command;
                }

            }
            catch (Exception ex)
            {
                TraceUtil.Error("Parse command error", ex);
                FinishCommand();
            }
        }

        private void HandleData(string request, ref int start)
        {
            if (_command.HandleData(request, ref start))
            {
                FinishCommand();
            }
            else
            {
                _state = CommandParseState.Data;
            }
        }

        private void FinishCommand()
        {
            lock (_lock)
            {
                _command = null;
                _state = CommandParseState.Idle;
                _commandParser.Cleanup();
            }
        }

        public void Dispose()
        {
            FinishCommand();
            _eventBus.Remove(EventKeys.ChannelClosed, _channelClosedHandler);
        }
    }
}
