using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Discord.Audio;
using Discord.Audio.Streams;

using RunBot.Services.Listening;
using RunBot.Services.VoiceRecognition;

namespace RunBot.Services
{
    public class StandardListener : IListener
    {
        bool listening = false;

        IAudioClient inputClient;

        public void SetInputClient(IAudioClient input) => this.inputClient = input;

        public Task ListenAsync(Action onRecognizedSpeech)
        {
            if (inputClient == null)
                throw new NullReferenceException("Input was null");
            listening = true;
            while (listening)
            {
                var streams = inputClient.GetStreams().Values;
                Parallel.ForEach(streams, stream => 
                {
                    //if (stream.AvailableFrames < 1)
                    //{
                    //    Thread.Sleep(50);
                    //    return;
                    //}
                    var recognizer = new AsyncVoiceRecognizer();
                    recognizer.RecognizeAsync((InputStream)stream, onRecognizedSpeech).Wait();
                });
            }
            return Task.CompletedTask;
        }

        public void Stop() => listening = false;
    }
}
