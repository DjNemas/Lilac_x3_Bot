using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands.Functions
{
    public class CommandsAll : CommandsHeader
    {
        public CommandsAll(DiscordSocketClient client, ContextInfo guildInfo) : base(client, guildInfo)
        {
        }

        public async Task CommandsGeneralAllAsync(string args = null)
        {
            // Check if Msg comes from Admin or General Channel
            bool check = false;
            if (this.ReadChannelGeneralAdmin())
            {
                check = true;
            }
            if (this.ReadChannelGeneralAll())
            {
                check = true;
            }
            if (!check) return;

            bool memberHasModRole = false;
            foreach (var item in Client.GetGuild(GuildInfo.GuildID).GetRole(ModRoleID).Members)
            {
                if (item.Id == Client.GetUser(GuildInfo.UserID).Id)
                {
                    memberHasModRole = true;
                }
            }

            if (args == null)
            {
                if (this.ReadChannelGeneralAdmin())
                {
                    await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + "\n>>> Nutze den Befehl wie folgt: `" +
                    Prefix + "commands <Modul> <Berechtigungsgruppe>`\nModule können mit `" + Prefix + "moduls` angezeigt werden.");
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + "\n>>> Nutze den Befehl wie folgt: `" +
                    Prefix + "commands <Modul> <Berechtigungsgruppe>`\nModule können mit `" + Prefix + "moduls` angezeigt werden.");
                }
                return;
            }

            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length < 2)
            {
                if (this.ReadChannelGeneralAdmin())
                {
                    await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
            }
            else if (countArgs.Length > 2)
            {
                if (this.ReadChannelGeneralAdmin())
                {
                    await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
            }
            else
            {
                if (this.ReadChannelGeneralAdmin() && memberHasModRole)
                {
                    await CommandsListMods(countArgs, memberHasModRole);
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await CommandsListAll(countArgs, memberHasModRole);
                }
            }
        }

        private async Task CommandsListMods(string[] countArgs, bool memberHasModRole)
        {
            if (countArgs[0] != "general" && countArgs[0] != "1337")
            {
                await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Falsches Modul\nBitte nutze " +
                    Prefix + "moduls um eine Liste der Module zu erhalten.");
                return;
            }
            if (countArgs[1] != "all" && countArgs[1] != "admin" && countArgs[1] != "mod")
            {
                await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                    Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
                return;
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
                await this.SendToGeneralChannelAdminAsync(Client.GetUser(GuildInfo.UserID).Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
                return;
            }
            if (countArgs[0] == "1337" && countArgs[1] == "all")
            {
                var str = new StringBuilder();
                str = HeaderCommandsList(str);
                str = Feature1337AllCommandsList(str);
                await this.SendToGeneralChannelAdminAsync(str.ToString());
            }
        }

        private async Task CommandsListAll(string[] countArgs, bool memberHasModRole)
        {
            if (countArgs[0] != "general" && countArgs[0] != "1337")
            {
                await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Falsches Modul\nBitte nutze " +
                    Prefix + "moduls um eine Liste der Module zu erhalten.");
                return;
            }
            if (countArgs[1] != "all" && countArgs[1] != "admin" && countArgs[1] != "mod")
            {
                await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                    Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
                return;
            }
            if (countArgs[0] == "general" && countArgs[1] == "all")
            {
                var str = new StringBuilder();
                str = HeaderCommandsList(str);
                str = GeneralAllCommandsList(str);
                await this.SendToGeneralChannelAllAsync(str.ToString());
            }
            if (countArgs[0] == "general" && countArgs[1] == "mod")
            {

                if (memberHasModRole == true)
                {
                    if (GenerelReadFromChannelAdminID == 0) return;
                    await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Bitte nutze den Befehl im `" + Client.GetGuild(GuildInfo.GuildID).GetChannel(GenerelReadFromChannelAdminID).Name + "` Channel.");
                }
                else
                {
                    await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
                }
                return;
            }
            if (countArgs[0] == "general" && countArgs[1] == "admin")
            {
                await this.SendToGeneralChannelAllAsync(Client.GetUser(GuildInfo.UserID).Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
                return;
            }
            if (countArgs[0] == "1337" && countArgs[1] == "all")
            {
                var str = new StringBuilder();
                str = HeaderCommandsList(str);
                str = Feature1337AllCommandsList(str);
                await this.SendToGeneralChannelAllAsync(str.ToString());
            }
        }

    }
}
