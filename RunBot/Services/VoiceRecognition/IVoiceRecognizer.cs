using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Discord.Audio.Streams;

namespace RunBot.Services.VoiceRecognition
{
    public interface IVoiceRecognizer
    {
        Task RecognizeAsync(InputStream audio, Action onRecognized);

        void Stop();
    }
}
