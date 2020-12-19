using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Audio;

namespace RunBot.Services.Audio
{
    /// <summary>
    /// Only supports s16le
    /// </summary>
    public class BasicAudioProvider : AudioProvider, IAudioProvider
    {
        public override Task PlayFileAsync(string path) => PlayRawFileAsync(path);

        public override async Task PlayRawFileAsync(string path)
        {
            using(var file = File.OpenRead(path))
            using(var output = CreateOutput())
            {
                await file.CopyToAsync(output).ConfigureAwait(false);
            }
        }
    }
}
