using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RunBot.Services.VoiceRecognition
{
    public interface IVoiceRecognizer
    {
        Task RecognizeAsync(Stream audio, Action onRecognized);
    }
}
