using System;
using System.Threading.Tasks;

using RunBot.Services.Logging;

namespace RunBot
{
    class Program
    {
        static async Task Main()
        {
            var bot = new Bot(new Discord.WebSocket.DiscordSocketConfig() 
            {
                LogLevel = Discord.LogSeverity.Debug
            }, IoC.Kernel.Get<CommandHandlers.ICommandHandler>(), IoC.Kernel.Get<IAsyncLogger>());

            await bot.StartAsync();
            await bot.SetGameAsync("RUN");

            await Task.Delay(-1);
        }
    }
}
