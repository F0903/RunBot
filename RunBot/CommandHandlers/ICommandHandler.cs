using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace RunBot.CommandHandlers
{
    public interface ICommandHandler
    {
        IDiscordClient HandlerClient { set; }

        Task OnCommand(SocketMessage message);

    }
}
