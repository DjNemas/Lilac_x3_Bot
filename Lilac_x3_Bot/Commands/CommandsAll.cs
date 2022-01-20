//using Discord;
//using Discord.Commands;
//using System;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lilac_x3_Bot.Commands
//{
//    public class CommandsAll : CommandsHeaderChat
//    {
//        [Command("moduls")]
//        public async Task ModulsAsync()
//        {
//            bool check = ReadChannelGeneralAll();
//            if (!check) return;

//            StringBuilder str = new StringBuilder();
//            str.AppendLine(Context.User.Mention + "Aktuell gibt es Folgende Module:");
//            str.AppendLine("Modul: `General`");
//            str.AppendLine("Berechtigungsgruppen: `All` `Admin` `Mod`");
//            str.AppendLine();
//            str.AppendLine("Modul: `1337`");
//            str.AppendLine("Berechtigungsgruppen: `All`");

//            await SendToGeneralChannelAllAsync(str.ToString());
//        }

//        [Command("credits")]
//        public async Task CreditsAsync()
//        {
//            bool check = this.ReadChannelGeneralAll();
//            if (!check) return;

//            var str = new StringBuilder();
//            str.AppendLine(Context.User.Mention);
//            str.AppendLine("__Credits:__");
//            str.AppendLine("Der Bot wurde von " + Context.Client.GetUser(123613862237831168) + " Programmiert.");
//            str.AppendLine("Der Source Code ist auf https://github.com/DjNemas/ zu finden.");
//            str.AppendLine("Kontaktiert mich doch gerne bei Fragen oder Problemen :) ");
//            str.AppendLine();
//            str.AppendLine("Ein Riesen Dankeschön geht an " + Context.Client.GetUser(308716816593584128) + " für das stundenlange Testen am Bot!");
//            str.AppendLine("Dank deiner Hilfe geht das Development viel schneller! <a:LilacxLoveGIF:708464221037527110>");

//            await this.SendToGeneralChannelAllAsync(str.ToString());
//        }

//        [Command("version")]
//        public async Task VersionAsync()
//        {
//            bool check = this.ReadChannelGeneralAll();
//            if (!check) return;

//            var str = new StringBuilder();
//            str.AppendLine(">>> Aktuelle Version: " + this.Version);
//            str.AppendLine("Das Updatelog ist nun hier zu finden: https://github.com/DjNemas/Lilac_x3_Bot");

//            await this.SendToGeneralChannelAllAsync(str.ToString());
//        }
//    }
//}
