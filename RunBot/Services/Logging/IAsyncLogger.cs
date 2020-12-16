using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RunBot.Services.Logging
{
    public readonly ref struct LogMsg
    {
        public LogMsg(int severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        public LogMsg(Discord.LogMessage logMessage)
        {
            Severity = (int)logMessage.Severity;
            Message = logMessage.Message;
        }

        public readonly int Severity;
        public readonly string Message;
    }

    public interface IAsyncLogger
    {
        ValueTask LogAsync(LogMsg msg);
    }
}
