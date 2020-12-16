using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

namespace RunBot.Services.VoiceRecognition
{
    public class VoiceRecognitionModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IVoiceRecognizer>().To<AsyncVoiceRecognizer>();
        }
    }
}
