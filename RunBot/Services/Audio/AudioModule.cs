using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

namespace RunBot.Services.Audio
{
    public class AudioModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IAudioProvider>().To<StandardAudioProvider>();
        }
    }
}
