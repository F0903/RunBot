﻿using System;
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
        //TODO: Fix strange error with Ninject when using IAudioProvider here.
        public StandardListener(BasicAudioProvider audio)
        {
            this.audio = audio;
        }

        readonly IAudioProvider audio;

        CancellationTokenSource cancellationTokenSource;

        IAudioClient inputClient;


        public void SetInputClient(IAudioClient input) => this.inputClient = input;

        //NOTE: According to the GitHub Issues, receiving voice requires transmitting audio first.
        public async Task ListenAsync(Action onRecognizedSpeech)
        {
            if (inputClient == null)
                throw new NullReferenceException("Input was null");

            cancellationTokenSource = new CancellationTokenSource();

            audio.SetOutput(inputClient);
            await audio.PlaySilenceAsync(1); // Play audio so we can receive.

            var streams = inputClient.GetStreams().Values;
            Parallel.ForEach(streams, stream =>
            {
                var recognizer = new AsyncVoiceRecognizer();
                recognizer.RecognizeAsync((InputStream)stream, cancellationTokenSource.Token, onRecognizedSpeech).Wait();
                stream.Dispose();
            });
        }

        public void Stop() 
        { 
            cancellationTokenSource?.Cancel();
            inputClient?.StopAsync();
            inputClient?.Dispose();
        }
    }
}
