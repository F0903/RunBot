using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using RunBot.Services.Logging;

namespace RunBot.CommandHandlers
{
    public class AsyncCommandHandler : ICommandHandler
    {
        public AsyncCommandHandler(IAsyncLogger logger)
        {
            this.logger = logger;
            Init();
        }

        static readonly IServiceProvider serviceProvider = IoC.Kernel.GetKernelAsServiceProvider();

        readonly IAsyncLogger logger;

        public IDiscordClient HandlerClient { private get; set; }

        readonly CommandService commands = new CommandService(new CommandServiceConfig
        {
            CaseSensitiveCommands = false,
            DefaultRunMode = RunMode.Async
        });

        private Task Init()
        {
            commands.CommandExecuted += async (info, ctx, result) => await logger.LogAsync(
                new LogMsg(result.IsSuccess ? 5 : 1,
                result.IsSuccess ?
                "Executed command successfully." :
                $"Error in command: {result.ErrorReason}"));

            commands.AddModulesAsync(Assembly.GetExecutingAssembly(), serviceProvider);

            return Task.CompletedTask;
        }

        // Called from OnMessage event.
        public async Task OnCommand(SocketMessage message)
        {
            if (HandlerClient == null) throw new NullReferenceException("HandlerClient is not initialized.");

            if (!(message is SocketUserMessage msg)) // If the message isn't a SockerUserMessage, return.
                return;

            int argPos = 0;
            if (!msg.HasStringPrefix(Settings.CommandPrefix, ref argPos)) // Does the message have our prefix?
                return;

            var ctx = new CommandContext(HandlerClient, msg);
            var result = await commands.ExecuteAsync(ctx, argPos, serviceProvider);
            if (!result.IsSuccess)
                await logger.LogAsync(new LogMsg(1, result.ErrorReason)).ConfigureAwait(false);
        }
    }
}
