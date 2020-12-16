using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

namespace RunBot.Commands
{
    public class MiscCommands : ModuleBase<CommandContext>
    {
        [Command("Ping")]
        public async Task Ping() => await ReplyAsync("Pong!");
    }
}
