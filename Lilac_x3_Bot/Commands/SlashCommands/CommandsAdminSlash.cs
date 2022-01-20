using Discord;
using Discord.Interactions;
using Lilac_x3_Bot.Commands.Functions;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands.SlashCommands
{
    public class CommandsAdminSlash : CommandsHeaderSlash
    {
        [SlashCommand("commandsadmin", "Eine Liste voller Kommandos.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CommandsGeneralAdminSlashAsync([Choice("General","general")] string modul)
        {
            LoadCommandsHeaderSlash();
            string args = modul + " admin";
            bool isInRightChannel = await this.Admin.CommandsGeneralAdminChatAsync(args);
            if (!isInRightChannel)
                await RespondAsync("Der Befehl darf nicht in diesem Channel genutzt werden.");
            else
            {
                await DeferAsync();
                await DeleteOriginalResponseAsync();
            }
            
        }
    }
}
