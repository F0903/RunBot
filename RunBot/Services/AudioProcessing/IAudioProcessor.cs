using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBot.Services.AudioProcessing
{
    public interface IAudioProcessor
    {
        Task<Stream> ProcessAudioFileToStream(string inputPath, string outputFormat, string inputFormat = null);
    }
}
