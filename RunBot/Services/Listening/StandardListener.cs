using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Discord.Audio;

using RunBot.Services.Listening;
using RunBot.Services.VoiceRecognition;

namespace RunBot.Services
{
    public class StandardListener : IListener
    {
        public StandardListener(IVoiceRecognizer recognizer)
        {
            this.recognizer = recognizer;
        }

        readonly IVoiceRecognizer recognizer;

        bool listening = false;

        IEnumerable<AudioInStream> inputs;

        public void SetInputs(IEnumerable<AudioInStream> inputs) => this.inputs = inputs;

        public Task ListenAsync(Action onRecognizedSpeech)
        {
            if (inputs == null)
                throw new NullReferenceException("Inputs were null");
            listening = true;
            while (listening)
            {
                foreach (var stream in inputs)
                {
                    if (stream.AvailableFrames > 1)
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                    recognizer.RecognizeAsync(stream, onRecognizedSpeech);
                }
            }
            return Task.CompletedTask;
        }

        public void Stop() => listening = false;
    }
}
