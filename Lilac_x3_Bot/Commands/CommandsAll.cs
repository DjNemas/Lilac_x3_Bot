using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{

    public class CommandsAll : CommandHeader
    {

        [Command("commands")]
        [RequireUserPermission(Discord.ChannelPermission.ManageChannels)]
        public async Task CommandsAsync()
        {
            bool check = ReadChannelGeneral();
            if (!check) return;

            var str = new StringBuilder();
            str.AppendLine(Context.User.Mention + "Kommando Liste\n");
            str.AppendLine(">>> __Berechtigung: Für Alle | Modul: General__");
            str.AppendLine("`!commands` Eine Liste voller Kommandos.");
            str.AppendLine("`!credits` Zeigt die Credits an.");
            str.AppendLine();
            str.AppendLine("__Berechtigung: Nur Serverweite Administratoren | Modul General__");
            str.AppendLine("`!prefix <prefix>` Hiermit stellst du den Prefix ein (nur 1 Zeichen erlaubt).");
            str.AppendLine("`!restart` Hiermit wird der Bot neugestartet. Bitte nur verwenden, wenn spezielle Einstellungen wie Prefix geändert wurde oder bei Problemen!");
            str.AppendLine("`!setoutputchannelgeneral <ChannelID>` Hier soll der Bot alle `Modul General` ausgaben senden. Bei ID 0 kommen die ausgaben immer im selben Chat!");
            str.AppendLine("`!setoutputchannel1337 <ChannelID>` Hier soll der Bot alle `Modul 1337` ausgaben senden. Bei ID 0 kommen die ausgaben immer im selben Chat!");
            str.AppendLine("`!setinputchannelgeneral <channelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General, bei channelID 0 wird überall gelauscht.");
            str.AppendLine("`!setinputchannel1337commands` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul 1337, bei channelID 0 wird überall gelauscht.");
            str.AppendLine("`!setinputchannel1337listen` In dem Channel wird nach 1337 und @1337 gelauscht und zählt nur dort mit. Bei channelID 0 wird in jedem Channel gelauscht.");
            str.AppendLine();
            str.AppendLine("__Berechtigung: Für Alle | Modul 1337__");
            str.AppendLine("`!1337info` Eine kleine Info zum Feature 1337.");
            str.AppendLine("`!1337streak` Zeigt deine aktuelle und höchste Streak an.");
            str.AppendLine("`!1337count` Zeigt deine gesamten gezählten Zählungen seit dato an.");
            str.AppendLine("`!1337hstreak` Zeigt die Top 10, geordnet nach aktuell höchster Streak und Namen, an.");
            str.AppendLine("`!1337hcount` Zeigt die Top 10, geordnet nach aktuell gesamten gezählten Zählungen und Namen, an.");
            str.AppendLine("`!1337highscore` Zeigt die Top 10, geordnet nach aktuell höchster Streak und Namen, sowie allen restlichen Informationen an.");

            await SendToGeneralChannelAsync(str.ToString());
        }

        [Command("credits")]
        [RequireUserPermission(Discord.ChannelPermission.ManageChannels)]
        public async Task CreditsAsync()
        {
            bool check = ReadChannelGeneral();
            if (!check) return;

            var str = new StringBuilder();
            str.AppendLine(Context.User.Mention);
            str.AppendLine("__Credits:__");
            str.AppendLine("Der Bot wurde von " + Context.Client.GetUser(123613862237831168) + " Programmiert.");
            str.AppendLine("Der Source Code ist auf https://github.com/DjNemas/ zu finden.");
            str.AppendLine("Kontaktiert mich doch gerne bei Fragen oder Problemen :) ");

            await SendToGeneralChannelAsync(str.ToString());
        }


    }
}
