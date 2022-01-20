using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands.ChatCommands
{
    public class CommandsAllChat : CommandsHeaderChat
    {
        [Command("commands")]
        public async Task CommandGeneralAllAsync([Remainder] string args = null)
        {
            LoadCommandsHeaderChat();
            await All.CommandsGeneralAllAsync(args);
        }

    }
}
