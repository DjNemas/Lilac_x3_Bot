using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands.Functions
{
    public struct ContextInfo
    {
        public readonly ulong GuildID;
        public readonly ulong ChannelID;
        public readonly ulong UserID;

        public ContextInfo(ulong guildID, ulong channelID, ulong userID)
        { 
            this.GuildID = guildID;
            this.ChannelID = channelID;
            this.UserID = userID;
        }
    }

    public class CommandsHeader
    {
        // Member
        public readonly ConfigXML _configXML = new ConfigXML();
        public readonly Tools t = new Tools();
        public readonly DatabaseInit dbClass = new DatabaseInit();

        protected readonly DiscordSocketClient Client;

        protected ContextInfo GuildInfo;

        protected static char Prefix;
        public static ulong Feature1337WriteIntoChannelID { get; protected set; }
        public static ulong Feature1337ReadFromChannelID { get; protected set; }
        public static ulong Feature1337ListenFromChannelID { get; protected set; }
        public static ulong GenerelWriteIntoChannelAllID { get; protected set; }
        public static ulong GenerelWriteIntoChannelAdminID { get; protected set; }
        public static ulong GenerelReadFromChannelAllID { get; protected set; }
        public static ulong GenerelReadFromChannelAdminID { get; protected set; }
        public static ulong ModRoleID { get; protected set; }

        public CommandsHeader()
        {
            // Load Config Data
            LoadConfigData();
        }

        public CommandsHeader(DiscordSocketClient client, ContextInfo guildInfo)
        {
            // Set Discord Client
            Client = client;
            // Get Guild Context Informations from CurrentCommand;
            GuildInfo = guildInfo;

            // Load Config Data
            LoadConfigData();
        }

        private void LoadConfigData()
        {
            // Get Prefix
            ConfigXML configXML = new ConfigXML();
            Prefix = configXML.GetPrefix();

            // Module 1337
            Feature1337WriteIntoChannelID = this._configXML.GetChannelUID("Feature1337", "WriteIntoChannel");
            Feature1337ReadFromChannelID = this._configXML.GetChannelUID("Feature1337", "ReadFromChannel");
            Feature1337ListenFromChannelID = this._configXML.GetChannelUID("Feature1337", "Listen1337FromChannel");
            //Module General
            GenerelWriteIntoChannelAllID = this._configXML.GetChannelUID("General", "WriteIntoChannelAll");
            GenerelWriteIntoChannelAdminID = this._configXML.GetChannelUID("General", "WriteIntoChannelAdmin");
            GenerelReadFromChannelAllID = this._configXML.GetChannelUID("General", "ReadFromChannelAll");
            GenerelReadFromChannelAdminID = this._configXML.GetChannelUID("General", "ReadFromChannelAdmin");
            // ModRoleID
            ModRoleID = this._configXML.GetChannelUID("General", "ModRoleID");
        }

        internal ulong GetFeature1337ListenFromChannelID()
        {
            return Feature1337ListenFromChannelID;
        }

        // For Listen on General Commands "All"
        internal bool ReadChannelGeneralAll()
        {
            if (GenerelReadFromChannelAllID == GuildInfo.ChannelID || GenerelReadFromChannelAllID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // For Listen on General Commands "Admin"
        internal bool ReadChannelGeneralAdmin()
        {
            if (GenerelReadFromChannelAdminID == GuildInfo.ChannelID || GenerelReadFromChannelAdminID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // For Listen on all 1337 Commands
        internal bool ReadChannel1337()
        {
            if (Feature1337ReadFromChannelID == GuildInfo.ChannelID || Feature1337ReadFromChannelID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // For Listen on all 1337 Counts
        internal bool ReadChannel1337Listen()
        {
            if (Feature1337ListenFromChannelID == GuildInfo.ChannelID || Feature1337ListenFromChannelID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Send to 1337 Channel
        internal async Task SendTo1337ChannelAsync(string msg)
        {
            if (Feature1337WriteIntoChannelID == 0)
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(GuildInfo.ChannelID).SendMessageAsync(msg);
            }
            else
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(Feature1337WriteIntoChannelID).SendMessageAsync(msg);
            }
        }
        protected async Task SendTo1337ChannelAsync(Embed embed)
        {
            if (Feature1337WriteIntoChannelID == 0)
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(GuildInfo.GuildID).SendMessageAsync(null, false, embed);
            }
            else
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(Feature1337WriteIntoChannelID).SendMessageAsync(null, false, embed);
            }
        }

        // Send to General Channel "Admin"
        internal async Task SendToGeneralChannelAdminAsync(string msg)
        {
            if (GenerelWriteIntoChannelAdminID == 0)
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(GuildInfo.ChannelID).SendMessageAsync(msg);
            }
            else
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(GenerelWriteIntoChannelAdminID).SendMessageAsync(msg);
            }
        }

        // Send to General Channel "All"
        internal async Task SendToGeneralChannelAllAsync(string msg)
        {
            if (GenerelWriteIntoChannelAllID == 0)
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(GuildInfo.ChannelID).SendMessageAsync(msg);
            }
            else
            {
                await Client.GetGuild(GuildInfo.GuildID).GetTextChannel(GenerelWriteIntoChannelAllID).SendMessageAsync(msg);
            }
        }

        // Restarts the Bot
        /*protected async Task RestartBot()
        {
            await this.SendToGeneralChannelAdminAsync("Ich starte kurz neu! Bitte gib mir einen kleinen moment. Dein Meow 1337 Bot <3");
            for (int i = 5; i >= 0; i--)
            {
                if (i > 0)
                {
                    await this.SendToGeneralChannelAdminAsync("Noch " + i.ToString() + " sec.");

                }
                else if (i == 0)
                {
                    await this.SendToGeneralChannelAdminAsync("Bis gleich o/");
                }
                Thread.Sleep(1000);
            }
            string myApp = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            string tmpMyApp = "";
            for (int i = 0; i < myApp.Length; i++)
            {

                if (i < 8)
                {
                    continue;
                }
                if (i == myApp.Length - 1 || i == myApp.Length - 3)
                {
                    tmpMyApp += "e";
                }
                else if (i == myApp.Length - 2)
                {
                    tmpMyApp += "x";
                }
                else
                {
                    tmpMyApp += myApp[i];
                }
            }
            System.Diagnostics.Process.Start(tmpMyApp);
            Environment.Exit(0);
        }*/

        // Set ID in config File
        protected async Task SetOutputID(string args, string commandname, string modul, string group, string modulID)
        {
            // need a argument
            if (args != null)
            {
                string[] countArgs = args.Split(' ');
                // check is exact one argument
                if (countArgs.Length != 1)
                {
                    await this.SendToGeneralChannelAdminAsync("Zu viele Agumente! Bitte `" + Prefix + commandname + " <channelid>` angeben.");
                }
                else
                {
                    ulong ulongID = GuildInfo.ChannelID;
                    // if not only numbers return
                    try
                    {
                        ulongID = Convert.ToUInt64(args);
                    }
                    catch (Exception)
                    {
                        await this.SendToGeneralChannelAdminAsync("Die ID besteht nicht nur aus Zahlen!");
                        return;
                    }

                    // check if channel id exist
                    var allChannel = Client.GetGuild(GuildInfo.GuildID).Channels;
                    var allRoles = Client.GetGuild(GuildInfo.GuildID).Roles;
                    bool exist = false;
                    // Check all Channel IDs
                    foreach (var item in allChannel)
                    {
                        if (ulongID == item.Id)
                        {
                            exist = true;
                            break;
                        }
                    }
                    // Check all Roles IDs
                    foreach (var item in allRoles)
                    {
                        if (ulongID == item.Id)
                        {
                            exist = true;
                            break;
                        }
                    }
                    // id right ? change configXML : notice user channel doen't exist
                    if (exist || ulongID == 0)
                    {
                        ConfigXML config = new ConfigXML();

                        config.ChangeWriteIntoChannelID(ulongID, modul, modulID);
                        if (ulongID == 0)
                        {
                            if (modulID == "ModRoleID")
                            {
                                await this.SendToGeneralChannelAdminAsync("Die ModRole wurde zurück gesetzt.");
                            }
                            else
                            {
                                await this.SendToGeneralChannelAdminAsync("Der Channel wurde auf Standardeinstellung gesetzt.");
                                await this.SendToGeneralChannelAdminAsync("Ich Lese und Antworte jetzt immer im selben Channel.");
                            }

                        }
                        else
                        {
                            if (modulID == "ModRoleID")
                            {
                                await this.SendToGeneralChannelAdminAsync("Du hast die ModRole `" + Client.GetGuild(GuildInfo.GuildID).GetRole(ulongID) + "` gesetzt");
                            }
                            else
                            {
                                await this.SendToGeneralChannelAdminAsync("Der Channel `" + Client.GetGuild(GuildInfo.GuildID).GetChannel(ulongID).Name +
                                "` wurde für das Modul `" + modul + " " + group + "` gesetzt.");
                            }

                        }
                    }
                    else
                    {
                        await this.SendToGeneralChannelAdminAsync("Diese ID existiert nicht.");
                    }
                }
            }
            else
            {
                await this.SendToGeneralChannelAdminAsync("Du hast zu wenig Argumente angebene. Bitte nutze den Befehl wie folgt: `" + Prefix + commandname + " <channelid>`");
            }
        }

        // Commandlist Header
        protected StringBuilder HeaderCommandsList(StringBuilder str)
        {
            str.AppendLine(Client.GetUser(GuildInfo.UserID).Mention + " Kommando Liste\n");
            return str;
        }

        // Commandlist Modul: General | Permission: ALL
        protected StringBuilder GeneralAllCommandsList(StringBuilder str)
        {
            str.AppendLine(">>> __Berechtigung: Für Alle | Modul: General__");
            str.AppendLine("`" + Prefix + "commands` Eine Liste voller Kommandos.");
            str.AppendLine("`" + Prefix + "moduls` Eine Liste von Modulen und deren Berechtigungsgruppen.");
            str.AppendLine("`" + Prefix + "credits` Zeigt die Credits an.");
            str.AppendLine("`" + Prefix + "version` Zeigt die Aktuelle Version und UpdateLog des Bots an.");
            str.AppendLine();
            return str;
        }

        // Commandlist Modul: General | Permission: Mod
        protected StringBuilder GeneralModCommandsList(StringBuilder str)
        {
            str.AppendLine("__Berechtigung: Moderatoren Rolle | Modul General__");
            str.AppendLine("`" + Prefix + "showusertoday <UserID>` Gibt alle 1337 Stats für den angegebenen User von Heute aus.");
            str.AppendLine("`" + Prefix + "showuseryesterday <UserID>` Gibt alle 1337 Stats für den angegebenen User von Gestern aus.");
            str.AppendLine("`" + Prefix + "edituser <UserID> <Counter_Streak> <Counter_Highest_Streak> <CounterAll> <Last_Date>` Ermöglicht das Editieren der Stats vom angegebenen User.");
            str.AppendLine("`" + Prefix + "rollback <DD> <MM> <YYYY>` Spielt ein Backup vom Tag DD MM YYYY auf.");
            str.AppendLine("`" + Prefix + "countuser <@User>` Zählt den User auch außerhalb der Event Zeit.");
            return str;
        }

        // Commandlist Modul: General | Permission: Admin
        protected StringBuilder GeneralAdminCommandsList(StringBuilder str)
        {
            str.AppendLine("__Berechtigung: Nur Serverweite Administratoren | Modul General__");
            str.AppendLine("`" + Prefix + "prefix <prefix>` Hiermit stellst du den Prefix ein (nur 1 Zeichen erlaubt).");
            str.AppendLine("`" + Prefix + "restart` Hiermit wird der Bot neugestartet. Bitte nur verwenden, wenn spezielle Einstellungen wie Prefix geändert wurde oder bei Problemen!");
            str.AppendLine("`" + Prefix + "setoutputchannelgeneralall <ChannelID>` Hier soll der Bot alle `Modul General All` ausgaben senden.");
            str.AppendLine("`" + Prefix + "setinputchannelgeneralall <ChannelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General All.");
            str.AppendLine("`" + Prefix + "setoutputchannelgeneraladmin <ChannelID>` Hier soll der Bot alle `Modul General Admin` ausgaben senden.");
            str.AppendLine("`" + Prefix + "setinputchannelgeneraladmin <ChannelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General Admin.");
            str.AppendLine("`" + Prefix + "setoutputchannel1337 <ChannelID>` Hier soll der Bot alle `Modul 1337` ausgaben senden.");
            str.AppendLine("`" + Prefix + "setinputchannel1337commands <ChannelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul 1337.");
            str.AppendLine("`" + Prefix + "setinputchannel1337listen <ChannelID>` In dem Channel wird nach 1337 und @1337 gelauscht und zählt nur dort mit.");
            str.AppendLine("`" + Prefix + "setmodrole <RoleID>` Hiermit wird die ModRole festgelegt, mit der Mod Commands genutzt werden können.");
            return str;
        }

        // Commandlist Modul: 1337 | Permission: ALL
        protected StringBuilder Feature1337AllCommandsList(StringBuilder str)
        {
            str.AppendLine("__Berechtigung: Für Alle | Modul 1337__");
            str.AppendLine("`" + Prefix + "1337info` Eine kleine Info zum Feature 1337.");
            str.AppendLine("`" + Prefix + "1337streak` Zeigt deine aktuelle und höchste Streak an.");
            str.AppendLine("`" + Prefix + "1337count` Zeigt deine gesamten gezählten Zählungen seit dato an.");
            str.AppendLine("`" + Prefix + "1337hstreak` Zeigt die Top 10, geordnet nach aktuell höchster Streak und Namen, an.");
            str.AppendLine("`" + Prefix + "1337hcount` Zeigt die Top 10, geordnet nach aktuell gesamten gezählten Zählungen und Namen, an.");
            str.AppendLine("`" + Prefix + "1337highscore <Anzahl User (1-30)>` Zeigt die Anzahl User, geordnet nach aktuell höchster Streak und Namen, sowie allen restlichen Informationen, als Highscoreliste an.");
            return str;
        }
    }
}
