using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

namespace RunBot.Services.AudioProcessing
{
    public class AudioProcessingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IAudioProcessor>().To<FFmpegAudioProcessor>();
        }
    }
}
