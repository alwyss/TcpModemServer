using System;
using Framework.Utilities.Helpers;

namespace Framework.Utilities.Console
{
    public class ConsoleInitializer
    {
        public static void Initialize()
        {
            ConsoleMenuHelper.DisableSystemCloseMenu();

            System.Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
            };

            if (!ConsoleModeHelper.DisableQuickEdit())
            {
                throw new InvalidOperationException("Failed to disable console quick edit mode.");
            }
        }
    }
}
