using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
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
            bool check = ReadChannelGeneral();
            if (!check) return;

            if (args != null)
            {
                string[] countArgs = args.Split(' ');

                if (countArgs.Length != 1)
                {
                    await SendToGeneralChannelAsync("Zu viele Agumente! Bitte `"+ Prefix + "prefix < prefix >` angeben.");

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
                    await SendToGeneralChannelAsync("> Info: Der Befehl brauch ein `" + Prefix + "restart`");
                }
            }
            else
            {
                await SendToGeneralChannelAsync("Du hast zu wenig Argumente angebene. Bitte nutze den Befehl wie folgt: `" + Prefix + "prefix <prefix>`");
            }
        }

        [Command("restart")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RestartAsyc()
        {
            bool check = ReadChannelGeneral();
            if (!check) return;

            await RestartBot();
        }

        [Command("setoutputchannelgeneral")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetOutputChannelGeneralAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = ReadChannelGeneral();
            if (!check) return;

            await SetOutputID(args, "setoutputchannelgeneral", "General", "WriteIntoChannel");
        }

        [Command("setoutputchannel1337")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetOutputChannel1337Async([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = ReadChannelGeneral();
            if (!check) return;

            await SetOutputID(args, "setoutputchannel1337", "Feature1337", "WriteIntoChannel");
        }

        [Command("setinputchannelgeneral")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannelGeneralAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = ReadChannelGeneral();
            if (!check) return;

            await SetOutputID(args, "setinputchannelgeneral", "General", "ReadFromChannel");
        }

        [Command("setinputchannel1337commands")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannel1337ReadAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = ReadChannelGeneral();
            if (!check) return;

            await SetOutputID(args, "setinputchannel1337commands", "Feature1337", "ReadFromChannel");
        }


        [Command("setinputchannel1337listen")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannel1337ListenAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = ReadChannelGeneral();
            if (!check) return;
            await SetOutputID(args, "setinputchannel1337read", "Feature1337", "Listen1337FromChannel");
        }


    }
}
