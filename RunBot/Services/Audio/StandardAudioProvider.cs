using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Audio;

using RunBot.Services.AudioProcessing;

namespace RunBot.Services.Audio
{
    public readonly struct ClientChannelPair
    {
        public ClientChannelPair(IAudioClient client, IVoiceChannel channel) { this.client = client; this.channel = channel; }
        public readonly IAudioClient client;
        public readonly IVoiceChannel channel;
        public bool IsNull() => client == null || channel == null;
    }

    public class StandardAudioProvider : IAudioProvider
    {
        public StandardAudioProvider(IAudioProcessor processor) 
        {
            this.processor = processor;
        }

        readonly IAudioProcessor processor;

        ClientChannelPair output;

        public void SetOutput(ClientChannelPair output) => this.output = output;

        AudioOutStream CreateOutput()
        {
            return output.client.CreatePCMStream(AudioApplication.Music, 96 * 1024, 10, 0);
        }

        public async Task PlayFile(string path)
        {
            //if (output.IsNull()) TEMP
            //    throw new NullReferenceException("Output was null.");
            using (var input = await processor.ProcessAudioFileToStream(path, "s16le"))
            using (var output = CreateOutput())
            {
                await input.CopyToAsync(output);
            }
        }
    }
}
