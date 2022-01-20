using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands.SlashCommands
{
    public class CommandsAllSlash : CommandsHeaderSlash
    {
        [SlashCommand("commandsall", "Eine Liste voller Kommandos.")]
        public async Task CommandGeneralAllAsync([Choice("General", "general"), Choice("1337", "1337")] string modul)
        {
            LoadCommandsHeaderSlash();
            string args = modul + " all";
            await All.CommandsGeneralAllAsync(args);
            await DeferAsync();
            await DeleteOriginalResponseAsync();
        }
    }
}
