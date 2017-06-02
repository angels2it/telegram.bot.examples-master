using System;
using Bot.Services;
using Topshelf;
using Topshelf.Logging;

namespace Telegram.Bot.Examples.Echo
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseLog4Net("log4net.config");
                x.Service<TelegramService>(s =>
                {
                    s.ConstructUsing(name => new TelegramService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.OnException(ex =>
                {
                    MainService.Logger.Log(LoggingLevel.Error, ex);
                });
                x.SetDescription("TelegramBot");
                x.SetDisplayName("TelegramBot");
                x.SetServiceName("TelegramBot");
            });
        }

    }
}
