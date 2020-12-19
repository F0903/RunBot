using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Discord.Audio;
using Discord.Audio.Streams;

using RunBot.Services.Audio;
using RunBot.Services.Listening;
using RunBot.Services.VoiceRecognition;

namespace RunBot.Services
{
    public class StandardListener : IListener
    {
        public StandardListener(IAudioProvider audio)
        {
            this.audio = audio;
        }

        readonly IAudioProvider audio;

        bool listening = false;

        IAudioClient inputClient;

        public void SetInputClient(IAudioClient input) => this.inputClient = input;

        //NOTE: According to the GitHub Issues, receiving voice requires transmitting audio first.
        public async Task ListenAsync(Action onRecognizedSpeech)
        {
            if (inputClient == null)
                throw new NullReferenceException("Input was null");

            audio.SetOutput(new ClientChannelPair(inputClient, null));
            await audio.PlayFile("media/sniff.wav");

            listening = true;

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
            Console.Write("");
            //while (listening)
            //{

            //}
        }

        public void Stop() => listening = false;
    }
}
