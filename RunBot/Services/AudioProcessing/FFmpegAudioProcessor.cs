using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Audio;

namespace RunBot.Services.AudioProcessing
{
    public class FFmpegAudioProcessor : IAudioProcessor
    {
        ~FFmpegAudioProcessor()
        {
            ffmpeg?.Dispose();
        }

        Process ffmpeg;

        void StartFFmpeg(string arguments)
        {
            ffmpeg = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "ffmpeg.exe",
                    Arguments = arguments,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            ffmpeg.Start();
        }

        public Task<Stream> ProcessAudioFileToStream(string inputPath, string outputFormat, string inputFormat = null)
        {
            StartFFmpeg($"{(inputFormat != null ? $"-f {inputFormat}" : "")} -i {inputPath} -f {outputFormat} pipe:1");
            return Task.FromResult(ffmpeg.StandardOutput.BaseStream);
        }
    }
}
