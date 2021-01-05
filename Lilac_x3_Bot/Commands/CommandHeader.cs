using Discord;
using Discord.Commands;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Service;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandHeader : ModuleBase<SocketCommandContext>
    {
        // Member
        public  ConfigXML _configXML = new ConfigXML();
        public Tools t = new Tools();
        public DatabaseInit dbClass = new DatabaseInit();
        public char Prefix;
        public string Version;
        public static ulong Feature1337WriteIntoChannelID { get; set; }
        public static ulong Feature1337ReadFromChannelID { get; set; }
        public static ulong Feature1337ListenFromChannelID { get; set; }
        public static ulong GenerelWriteIntoChannelAllID { get; set; }
        public static ulong GenerelWriteIntoChannelAdminID { get; set; }
        public static ulong GenerelReadFromChannelAllID { get; set; }
        public static ulong GenerelReadFromChannelAdminID { get; set; }

        public static ulong ModRoleID { get; set; }

        public CommandHeader()
        {
            InitBot init = new InitBot();
            this.Version = init.GetVersion();

            CommandHandlingService pre = new CommandHandlingService();
            this.Prefix = pre.GetPrefix();

            //this.self = new CommandHeader();
            // Module 1337
            Feature1337WriteIntoChannelID = this._configXML.GetChannelUID("Feature1337","WriteIntoChannel");
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

        public ulong GetFeature1337ListenFromChannelID() 
        {
            return Feature1337ListenFromChannelID; 
        }

        // For Listen on General Commands "All"
            public bool ReadChannelGeneralAll()
        {
            if (GenerelReadFromChannelAllID == Context.Channel.Id || GenerelReadFromChannelAllID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadChannelGeneralAll(SocketCommandContext context)
        {
            if (GenerelReadFromChannelAllID == context.Channel.Id || GenerelReadFromChannelAllID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // For Listen on General Commands "Admin"
        public bool ReadChannelGeneralAdmin()
        {
            if (GenerelReadFromChannelAdminID == Context.Channel.Id || GenerelReadFromChannelAdminID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadChannelGeneralAdmin(SocketCommandContext context)
        {
            if (GenerelReadFromChannelAdminID == context.Channel.Id || GenerelReadFromChannelAdminID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // For Listen on all 1337 Commands
        public bool ReadChannel1337()
        {
            if (Feature1337ReadFromChannelID == Context.Channel.Id || Feature1337ReadFromChannelID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadChannel1337(SocketCommandContext context)
        {
            if (Feature1337ReadFromChannelID == context.Channel.Id || Feature1337ReadFromChannelID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // For Listen on all 1337 Counts
        public bool ReadChannel1337Listen(SocketCommandContext context)
        {
            if (Feature1337ListenFromChannelID == context.Channel.Id || Feature1337ListenFromChannelID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Send to 1337 Channel
        public async Task SendTo1337ChannelAsync(string msg)
        {
            if (Feature1337WriteIntoChannelID == 0)
            {
                await Context.Guild.GetTextChannel(Context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await Context.Guild.GetTextChannel(Feature1337WriteIntoChannelID).SendMessageAsync(msg);
            }
        }
        public async Task SendTo1337ChannelAsync(Embed embed)
        {
            if (Feature1337WriteIntoChannelID == 0)
            {
                await Context.Guild.GetTextChannel(Context.Channel.Id).SendMessageAsync(null,false,embed);
            }
            else
            {
                await Context.Guild.GetTextChannel(Feature1337WriteIntoChannelID).SendMessageAsync(null,false,embed);
            }
        }
        public async Task SendTo1337ChannelAsync(string msg, SocketCommandContext context)
        {
            if (Feature1337WriteIntoChannelID == 0)
            {
                await context.Guild.GetTextChannel(context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await context.Guild.GetTextChannel(Feature1337WriteIntoChannelID).SendMessageAsync(msg);
            }
        }
        // Send to General Channel "Admin"
        public async Task SendToGeneralChannelAdminAsync(string msg)
        {
            if (GenerelWriteIntoChannelAdminID == 0)
            {
                await Context.Guild.GetTextChannel(Context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await Context.Guild.GetTextChannel(GenerelWriteIntoChannelAdminID).SendMessageAsync(msg);
            }
        }
        public async Task SendToGeneralChannelAdminAsync(string msg, SocketCommandContext context)
        {
            if (GenerelWriteIntoChannelAdminID == 0)
            {
                await context.Guild.GetTextChannel(context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await context.Guild.GetTextChannel(GenerelWriteIntoChannelAdminID).SendMessageAsync(msg);
            }
        }
        // Send to General Channel "All"
        public async Task SendToGeneralChannelAllAsync(string msg)
        {
            if (GenerelWriteIntoChannelAllID == 0)
            {
                await Context.Guild.GetTextChannel(Context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await Context.Guild.GetTextChannel(GenerelWriteIntoChannelAllID).SendMessageAsync(msg);
            }
        }
        public async Task SendToGeneralChannelAllAsync(string msg, SocketCommandContext context)
        {
            if (GenerelWriteIntoChannelAllID == 0)
            {
                await context.Guild.GetTextChannel(context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await context.Guild.GetTextChannel(GenerelWriteIntoChannelAllID).SendMessageAsync(msg);
            }
        }

        public async Task RestartBot()
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
        }

        public async Task SetOutputID(string args, string commandname, string modul, string group, string modulID)
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
                    ulong ulongID = Context.Channel.Id;
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
                    var allChannel = Context.Guild.Channels;
                    var allRoles = Context.Guild.Roles;
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
                                await this.SendToGeneralChannelAdminAsync("Du hast die ModRole `" + Context.Guild.GetRole(ulongID) +  "` gesetzt");
                            }
                            else
                            {
                                await this.SendToGeneralChannelAdminAsync("Der Channel `" + Context.Guild.GetChannel(ulongID).Name +
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

        public StringBuilder HeaderCommandsList(StringBuilder str)
        {
            str.AppendLine(Context.User.Mention + " Kommando Liste\n");
            return str;
        }

        public StringBuilder GeneralAllCommandsList(StringBuilder str)
        {
            str.AppendLine(">>> __Berechtigung: Für Alle | Modul: General__");
            str.AppendLine("`" + this.Prefix + "commands` Eine Liste voller Kommandos.");
            str.AppendLine("`" + this.Prefix + "moduls` Eine Liste von Modulen und deren Berechtigungsgruppen.");
            str.AppendLine("`" + this.Prefix + "credits` Zeigt die Credits an.");
            str.AppendLine("`" + this.Prefix + "version` Zeigt die Aktuelle Version und UpdateLog des Bots an.");
            str.AppendLine();
            return str;
        }

        public StringBuilder GeneralAdminCommandsList(StringBuilder str)
        {
            str.AppendLine("__Berechtigung: Nur Serverweite Administratoren | Modul General__");
            str.AppendLine("`" + this.Prefix + "prefix <prefix>` Hiermit stellst du den Prefix ein (nur 1 Zeichen erlaubt).");
            str.AppendLine("`" + this.Prefix + "restart` Hiermit wird der Bot neugestartet. Bitte nur verwenden, wenn spezielle Einstellungen wie Prefix geändert wurde oder bei Problemen!");
            str.AppendLine("`" + this.Prefix + "setoutputchannelgeneralall <ChannelID>` Hier soll der Bot alle `Modul General All` ausgaben senden. Bei ID 0 kommen die ausgaben immer im selben Chat!");
            str.AppendLine("`" + this.Prefix + "setinputchannelgeneralall <ChannelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General All, bei channelID 0 wird überall gelauscht.");
            str.AppendLine("`" + this.Prefix + "setoutputchannelgeneraladmin <ChannelID>` Hier soll der Bot alle `Modul General Admin` ausgaben senden. Bei ID 0 kommen die ausgaben immer im selben Chat!");
            str.AppendLine("`" + this.Prefix + "setinputchannelgeneraladmin <ChannelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General Admin, bei channelID 0 wird überall gelauscht.");
            str.AppendLine("`" + this.Prefix + "setoutputchannel1337 <ChannelID>` Hier soll der Bot alle `Modul 1337` ausgaben senden. Bei ID 0 kommen die ausgaben immer im selben Chat!");
            str.AppendLine("`" + this.Prefix + "setinputchannel1337commands <ChannelID>` Der Bot hört nur in dem Channel auf alle Kommandos vom Modul 1337, bei channelID 0 wird überall gelauscht.");
            str.AppendLine("`" + this.Prefix + "setinputchannel1337listen <ChannelID>` In dem Channel wird nach 1337 und @1337 gelauscht und zählt nur dort mit. Bei channelID 0 wird in jedem Channel gelauscht.");
            str.AppendLine("`" + this.Prefix + "setmodroleid <RoleID>` Hiermit wird die ModRole festgelegt, mit der Mod Commands genutzt werden können.");
            return str;
        }

        public StringBuilder Feature1337AllCommandsList(StringBuilder str)
        {
            str.AppendLine("__Berechtigung: Für Alle | Modul 1337__");
            str.AppendLine("`" + this.Prefix + "1337info` Eine kleine Info zum Feature 1337.");
            str.AppendLine("`" + this.Prefix + "1337streak` Zeigt deine aktuelle und höchste Streak an.");
            str.AppendLine("`" + this.Prefix + "1337count` Zeigt deine gesamten gezählten Zählungen seit dato an.");
            str.AppendLine("`" + this.Prefix + "1337hstreak` Zeigt die Top 10, geordnet nach aktuell höchster Streak und Namen, an.");
            str.AppendLine("`" + this.Prefix + "1337hcount` Zeigt die Top 10, geordnet nach aktuell gesamten gezählten Zählungen und Namen, an.");
            str.AppendLine("`" + this.Prefix + "1337highscore` Zeigt die Top 10, geordnet nach aktuell höchster Streak und Namen, sowie allen restlichen Informationen an.");
            return str;
        }
    }
}
