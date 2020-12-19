﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Audio;

namespace RunBot.Services.Audio
{
    public abstract class AudioProvider : IAudioProvider
    {
        IAudioClient output;

        public void SetOutput(IAudioClient output) => this.output = output;

        protected AudioOutStream CreateOutput()
        {
            if (output == null)
                throw new NullReferenceException("You need to call SetOutput first.");
            return output.CreatePCMStream(AudioApplication.Music, 96 * 1024, 10, 0);
        }

        public Task PlaySilenceAsync(int seconds)
        {
            using (var output = CreateOutput())
            {
                const int sampleRate = 48000;
                const int channels = 2;
                const int bytePerSample = 2;
                for (int i = 0; i < sampleRate * bytePerSample * channels * seconds; i++)
                {
                    output.WriteByte(0);
                }
            }
            return Task.CompletedTask;
        }

        public abstract Task PlayRawFileAsync(string path);

        public abstract Task PlayFileAsync(string path);
    }
}