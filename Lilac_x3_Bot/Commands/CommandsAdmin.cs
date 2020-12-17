using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsAdmin : CommandHeader
    {

        [Command("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PrefixAsyc([Remainder] string args = null)
        {
            if (args != null)
            {
                string[] countArgs = args.Split(' ');

                if (countArgs.Length != 1)
                {
                    await SendToGeneralChannelAsync("Zu viele Agumente! Bitte `!prefix < prefix >` angeben.");

                }
                else if (countArgs[0].Length > 1)
                {
                    await SendToGeneralChannelAsync("Der Prefix ist zu lang. Nur ein Zeichen!");
                }
                else
                {
                    ConfigXML config = new ConfigXML();
                    config.ChangePrefix(countArgs[0][0]);
                    await SendToGeneralChannelAsync("Der Prefix wurde zu `" + countArgs[0][0] + "` geändert!");
                    await RestartBot();
                }
            }
        }

        [Command("shutdown")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ShutdownAsyc([Remainder] string args = null)
        {
            await SendTo1337ChannelAsync("Ich verabschiede mich mal o/");
            Environment.Exit(0);
        }
    }
}
