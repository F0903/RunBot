using System;
using System.Collections.Generic;
using System.Text;

using Ninject.Modules;

namespace RunBot.Services.Logging
{
    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IAsyncLogger>().To<StandardLogger>();
        }
    }
}
