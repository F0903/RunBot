using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Ninject;
using Ninject.Parameters;

namespace RunBot.IoC
{
    public static class Kernel
    {
        static Kernel() => kernel.Load(Assembly.GetCallingAssembly());

        readonly static IKernel kernel = new StandardKernel();

        public static IServiceProvider GetKernelAsServiceProvider() => kernel;

        public static T Get<T>(params IParameter[] paras) => kernel.Get<T>(paras);
    }
}
