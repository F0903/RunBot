using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using RunBot.Services.Logging;
using RunBot.CommandHandlers;

namespace RunBot
{
    class Bot
    {
        public Bot(DiscordSocketConfig config, ICommandHandler handler, IAsyncLogger logger)
        {
            this.client = new DiscordSocketClient(config);
            this.handler = handler;
            this.logger = logger;
            Init();
        }

        readonly DiscordSocketClient client;
        readonly ICommandHandler handler;
        readonly IAsyncLogger logger;

        private Task Init()
        {
            client.LoginAsync(TokenType.Bot, Settings.Token);
            client.Log += async x => await logger.LogAsync(new LogMsg(x));
            handler.HandlerClient = client;
            client.MessageReceived += handler.OnCommand;
            return Task.CompletedTask;
        }

        public async Task StartAsync()
        {
            await client.StartAsync();
        }

        public async Task StopAsync()
        {
            await client.StopAsync();
        }

        public async Task SetGameAsync(string gameName)
        {
            await client.SetGameAsync(gameName);
        }
    }
}
