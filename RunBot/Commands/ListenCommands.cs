using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using RunBot.Services.Listening;
using RunBot.Services.VoiceRecognition;

namespace RunBot.Commands
{
    public class ListenCommands : ModuleBase<CommandContext>
    {
        public ListenCommands(IListener listener)
        {
            this.listener = listener;
        }

        readonly IListener listener;

        [Command("Listen", RunMode = RunMode.Async)]
        public async Task Listen()
        {
            if (!(Context.User is SocketGuildUser user))
            {
                await ReplyAsync("Not socket guild user.");
                return;
            }

            var voiceChannel = user.VoiceChannel;
            var audioClient = await voiceChannel.ConnectAsync();
            listener.SetInputClient(audioClient);
            await listener.ListenAsync(() => ReplyAsync("I recognized 'run'!"));
        }
    }
}
