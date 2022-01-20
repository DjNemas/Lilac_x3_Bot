using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands.Functions
{

    public class CommandsAdmin : CommandsHeader
    {
        public CommandsAdmin(DiscordSocketClient client, ContextInfo guildInfo) : base(client, guildInfo)
        {

        }

        public async Task<bool> CommandsGeneralAdminChatAsync(string args = null)
        {
            bool check = this.ReadChannelGeneralAdmin();
            Console.WriteLine("Check: " + check);
            if (!check) return false;

            if (args == null)
            {
                await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + "\n>>> Nutze den Befehl wie folgt: `" +
                    Prefix + "commands <Modul> <Berechtigungsgruppe>`\nModule können mit `" + Prefix + "moduls` angezeigt werden.");
                return true;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length < 2)
            {
                await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
            }
            else if (countArgs.Length > 2)
            {
                await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
            }
            else
            {
                if (countArgs[0] != "general" && countArgs[0] != "1337")
                {
                    await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Falsches Modul\nBitte nutze " +
                        Prefix + "moduls um eine Liste der Module zu erhalten.");
                    return true;
                }
                if (countArgs[1] != "all" && countArgs[1] != "admin" && countArgs[1] != "mod")
                {
                    await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                        Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
                    return true;
                }
                if (countArgs[0] == "general" && countArgs[1] == "all")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralAllCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
                if (countArgs[0] == "general" && countArgs[1] == "mod")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralModCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
                if (countArgs[0] == "general" && countArgs[1] == "admin")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralAdminCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
                if (countArgs[0] == "1337" && countArgs[1] == "all")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = Feature1337AllCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
            }
            return true;
        }
    }
}
