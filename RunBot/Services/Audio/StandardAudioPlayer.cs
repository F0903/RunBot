using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <summary>
    /// Provides the option for audio conversion.
    /// </summary>
    public class StandardAudioPlayer : BaseAudioPlayer
    {
        public StandardAudioPlayer(IAudioProcessor processor) 
        {
            this.processor = processor;
        }

        readonly IAudioProcessor processor;
        
        public override async Task PlayFileAsync(string path)
        {
            using (var input = await processor.ProcessAudioFileToStream(path, "s16le"))
            using (var output = CreateOutput())
            {
                await input.CopyToAsync(output);
            }
        }

        public override async Task PlayRawFileAsync(string path)
        {
            using (var file = File.OpenRead(path))
            using (var output = CreateOutput())
            {
                await file.CopyToAsync(output).ConfigureAwait(false);
            }
        }
    }
}
