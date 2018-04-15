using System;

namespace Framework.Utilities.Tracing
{
    [Flags]
    public enum TracingOutput
    {
        None = 0,
        File = 1,
        Console = 2
    }
}