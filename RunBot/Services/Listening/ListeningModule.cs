using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

namespace RunBot.Services.Listening
{
    public class ListeningModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IListener>().To<StandardListener>();
        }
    }
}
