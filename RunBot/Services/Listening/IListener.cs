using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Audio;

namespace RunBot.Services.Listening
{
    public interface IListener
    {
        void SetInputs(IEnumerable<AudioInStream> inputs);
        Task ListenAsync(Action onRecognizedSpeech);
        void Stop();
    }
}
