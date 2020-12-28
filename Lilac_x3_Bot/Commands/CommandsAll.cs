using Discord;
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
            str.AppendLine("Berechtigungsgruppen: `All` `Admin`");
            str.AppendLine();
            str.AppendLine("Modul: `1337`");
            str.AppendLine("Berechtigungsgruppen: `All`");

            await SendToGeneralChannelAllAsync(str.ToString());
        }

        [Command("commands")]
        public async Task CommandsGeneralAllAsync([Remainder] string args = null)
        {
            bool check = this.ReadChannelGeneralAll();
            if (!check) return;

            if (args == null)
            {
                await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Du hast zu wenige Argumente angebene. Bitte nutze den Befehl wie folgt: `" +
                    this.Prefix + "commands <Modul> <Berechtigungsgruppe>`");
                return;
            }

            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length < 2)
            {
                await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
            }
            else if (countArgs.Length > 2)
            {
                await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
            }
            else
            {
                if (countArgs[0] != "general" && countArgs[0] != "1337")
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Falsches Modul\nBitte nutze " +
                        this.Prefix + "moduls um eine Liste der Module zu erhalten.");
                    return;
                }
                if (countArgs[1] != "all" && countArgs[1] != "admin")
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                        this.Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
                    return;
                }
                if (countArgs[0] == "general" && countArgs[1] == "admin")
                {
                    await this.SendToGeneralChannelAllAsync(Context.User.Mention + " Du bist nicht berechtigt diese Commandsliste einzusehen.");
                    return;
                }
                if (countArgs[0] == "general" && countArgs[1] == "all")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralAllCommandsList(str);
                    await this.SendToGeneralChannelAllAsync(str.ToString());
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
            str.AppendLine(">>> __Aktuelle Version: " + this.Version + "__");
            str.AppendLine();
            str.AppendLine("__Neu in dieser Version:__");
            str.AppendLine("Neu: " + this.Prefix + "version, " + this.Prefix + "credits und " + this.Prefix + "moduls.");
            str.AppendLine("Geändert: `" + this.Prefix + "commands` muss jetzt mit `" + this.Prefix + "commands <ModulName> <Berechtigungsgruppe>` aufgerufen werden.");
            str.AppendLine("Bugfixes (siehe Bufixes und bekannte Bugs).");
            str.AppendLine();
            str.AppendLine("__Bufixes und bekannte Bugs:__");
            str.AppendLine("Behoben: Wenn man den Prefix geändert hat, wurden die Prefixes beim Command `" + this.Prefix + "commands` nicht übernommen.");
            str.AppendLine("Behoben: Beim wechsel vom Prefix, wurde bei Usern die nicht Autorisiert für ein Command sind, eine falsche Fehlermeldung ausgegeben.");
            str.AppendLine("Behoben: Bot gab eine Nachricht aus, wenn User wiederholt beim Feature 1337 gezählt wurden.");
            str.AppendLine("Bekannt: In einigen Textausgaben, fehlen noch Emotes.");
            str.AppendLine();
            str.AppendLine("__Next Steps:__");
            str.AppendLine("Geplant: Tägliches Backup der Datenbank");
            str.AppendLine("Geplant: Log in Datei schreiben für zukünftige Bugs und überprüfungen der Zählungen.");
            str.AppendLine("Neues Feature: Bot gibt ein Ergebnis der täglichen Zählungen aus.");
            str.AppendLine("Neues Feature: Highscoreliste Variable aufrufen um x User anzuzeigen. (Limit vermutlich 20 - 30 User)");


            await this.SendToGeneralChannelAllAsync(str.ToString());
        }


    }
}
