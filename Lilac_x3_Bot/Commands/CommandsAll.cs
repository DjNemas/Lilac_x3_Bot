﻿using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsAll : CommandHeader
    {
        [Command("moduls")]
        public async Task ModulsAsync()
        {
            bool check = ReadChannelGeneralAll();
            if (!check) return;

            StringBuilder str = new StringBuilder();
            str.AppendLine(Context.User.Mention + "Aktuell gibt es Folgende Module:");
            str.AppendLine("Modul: `General`");
            str.AppendLine("Berechtigungsgruppen: `All` `Admin` `Mod`");
            str.AppendLine();
            str.AppendLine("Modul: `1337`");
            str.AppendLine("Berechtigungsgruppen: `All`");

            await SendToGeneralChannelAllAsync(str.ToString());
        }

        [Command("commands")]
        public async Task CommandsGeneralAllAsync([Remainder] string args = null)
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
            foreach (var item in Context.Guild.GetRole(ModRoleID).Members)
            {
                if (item.Id == Context.User.Id)
                {
                    memberHasModRole = true;
                }
            }

            if (args == null)
            {
                if (this.ReadChannelGeneralAdmin())
                {
                    await this.SendToGeneralChannelAdminAsync(Context.User.Mention + "\n>>> Nutze den Befehl wie folgt: `" +
                    this.Prefix + "commands <Modul> <Berechtigungsgruppe>`\nModule können mit `" + this.Prefix + "moduls` angezeigt werden.");
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + "\n>>> Nutze den Befehl wie folgt: `" +
                    this.Prefix + "commands <Modul> <Berechtigungsgruppe>`\nModule können mit `" + this.Prefix + "moduls` angezeigt werden.");
                }
                return;
            }

            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length < 2)
            {
                if (this.ReadChannelGeneralAdmin())
                {
                    await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
            }
            else if (countArgs.Length > 2)
            {
                if (this.ReadChannelGeneralAdmin())
                {
                    await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
                }
                else if (this.ReadChannelGeneralAll())
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
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

        [Command("credits")]
        public async Task CreditsAsync()
        {
            bool check = this.ReadChannelGeneralAll();
            if (!check) return;

            var str = new StringBuilder();
            str.AppendLine(Context.User.Mention);
            str.AppendLine("__Credits:__");
            str.AppendLine("Der Bot wurde von " + Context.Client.GetUser(123613862237831168) + " Programmiert.");
            str.AppendLine("Der Source Code ist auf https://github.com/DjNemas/ zu finden.");
            str.AppendLine("Kontaktiert mich doch gerne bei Fragen oder Problemen :) ");
            str.AppendLine();
            str.AppendLine("Ein Riesen Dankeschön geht an " + Context.Client.GetUser(308716816593584128) + " für das stundenlange Testen am Bot!");
            str.AppendLine("Dank deiner Hilfe geht das Development viel schneller! <a:LilacxLoveGIF:708464221037527110>");

            await this.SendToGeneralChannelAllAsync(str.ToString());
        }

        [Command("version")]
        public async Task VersionAsync()
        {
            bool check = this.ReadChannelGeneralAll();
            if (!check) return;

            var str = new StringBuilder();
            str.AppendLine(">>> Aktuelle Version: " + this.Version);
            str.AppendLine("Das Updatelog ist nun hier zu finden: https://github.com/DjNemas/Lilac_x3_Bot");

            await this.SendToGeneralChannelAllAsync(str.ToString());
        }

        public async Task CommandsListMods(string[] countArgs, bool memberHasModRole)
        {
            if (countArgs[0] != "general" && countArgs[0] != "1337")
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Falsches Modul\nBitte nutze " +
                    this.Prefix + "moduls um eine Liste der Module zu erhalten.");
                return;
            }
            if (countArgs[1] != "all" && countArgs[1] != "admin" && countArgs[1] != "mod")
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                    this.Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
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
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
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

        public async Task CommandsListAll(string[] countArgs, bool memberHasModRole)
        {
            if (countArgs[0] != "general" && countArgs[0] != "1337")
            {
                await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Falsches Modul\nBitte nutze " +
                    this.Prefix + "moduls um eine Liste der Module zu erhalten.");
                return;
            }
            if (countArgs[1] != "all" && countArgs[1] != "admin" && countArgs[1] != "mod")
            {
                await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                    this.Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
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
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Bitte nutze den Befehl im `" + Context.Guild.GetChannel(GenerelReadFromChannelAdminID).Name + "` Channel.");
                }
                else
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
                }
                return;
            }
            if (countArgs[0] == "general" && countArgs[1] == "admin")
            {
                await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
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
