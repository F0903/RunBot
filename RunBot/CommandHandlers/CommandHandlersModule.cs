using System;
using System.Collections.Generic;
using System.Text;

using Ninject.Modules;

namespace RunBot.CommandHandlers
{
    public class CommandHandlersModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ICommandHandler>().To<AsyncCommandHandler>();
        }
    }
}
