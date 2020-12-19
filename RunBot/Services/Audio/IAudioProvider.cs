﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Audio;

namespace RunBot.Services.Audio
{
    public interface IAudioProvider
    {
        void SetOutput(ClientChannelPair output);

        Task PlayFile(string path);
    }
}