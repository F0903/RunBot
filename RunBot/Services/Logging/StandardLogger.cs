using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RunBot.Services.Logging
{
    public class StandardLogger : IAsyncLogger
    {
        public ValueTask LogAsync(LogMsg msg)
        {
            Console.WriteLine($"<{msg.Severity}> {msg.Message}");
            return new ValueTask();
        }
    }
}
