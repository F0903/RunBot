using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using RunBot.Services.Audio;
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
            if (!(Context.User is IGuildUser user))
            {
                await ReplyAsync("Not a guild user.");
                return;
            }
            
            var voiceChannel = user.VoiceChannel;
            if(voiceChannel == null)
            {
                await ReplyAsync("Please connect to a voice channel.");
                return;
            }
            var audioClient = await voiceChannel.ConnectAsync();
            listener.SetInputClient(audioClient);
            await listener.ListenAsync(async () => 
            {
                var player = new BasicAudioProvider();
                player.SetOutput(audioClient);
                await player.PlayRawFileAsync("media/run.pcm");
            });
        }

        [Command("Stop")]
        public Task StopListen()
        {
            listener.Stop();
            return Task.CompletedTask;
        }
    }
}
