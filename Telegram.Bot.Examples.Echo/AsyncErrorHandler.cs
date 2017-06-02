using System;

namespace Telegram.Bot.Examples.Echo
{
    public static class AsyncErrorHandler
    {
        public static void HandleException(Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}