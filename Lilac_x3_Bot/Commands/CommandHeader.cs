using Discord.Commands;
using Lilac_x3_Bot.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandHeader : ModuleBase<SocketCommandContext>
    {
        // Member
        private ConfigXML _configXML = new ConfigXML();
        private Tools t = new Tools();
        public char Prefix;
        public static ulong Feature1337WriteIntoChannelID { get; set; }
        public static ulong Feature1337ReadFromChannelID { get; set; }
        public static ulong GenerelWriteIntoChannelID { get; set; }
        public static ulong GenerelReadFromChannelID { get; set; }
        public static ulong Listen1337FFromChannelID { get; set; }

        public CommandHeader()
        {
            CommandHandlingService pre = new CommandHandlingService();
            this.Prefix = pre.GetPrefix();

            //this.self = new CommandHeader();
            Feature1337WriteIntoChannelID = this._configXML.GetChannelUID("Feature1337","WriteIntoChannel");
            Feature1337ReadFromChannelID = this._configXML.GetChannelUID("Feature1337", "ReadFromChannel");
            GenerelWriteIntoChannelID = this._configXML.GetChannelUID("General", "WriteIntoChannel");
            GenerelReadFromChannelID = this._configXML.GetChannelUID("General", "ReadFromChannel");
            Listen1337FFromChannelID = this._configXML.GetChannelUID("Feature1337", "Listen1337FromChannel");

        }

        // For Listen on all General Commands
        public bool ReadChannelGeneral()
        {
            if (GenerelReadFromChannelID == Context.Channel.Id || GenerelReadFromChannelID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadChannelGeneral(SocketCommandContext context)
        {
            if (GenerelReadFromChannelID == context.Channel.Id || GenerelReadFromChannelID == 0)
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
            if (Listen1337FFromChannelID == context.Channel.Id || Listen1337FFromChannelID == 0)
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
        // Send to General Channel
        public async Task SendToGeneralChannelAsync(string msg)
        {
            if (GenerelWriteIntoChannelID == 0)
            {
                await Context.Guild.GetTextChannel(Context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await Context.Guild.GetTextChannel(GenerelWriteIntoChannelID).SendMessageAsync(msg);
            }
        }
        public async Task SendToGeneralChannelAsync(string msg, SocketCommandContext context)
        {
            if (GenerelWriteIntoChannelID == 0)
            {
                await context.Guild.GetTextChannel(context.Channel.Id).SendMessageAsync(msg);
            }
            else
            {
                await context.Guild.GetTextChannel(GenerelWriteIntoChannelID).SendMessageAsync(msg);
            }
        }

        public async Task RestartBot()
        {
            await SendToGeneralChannelAsync("Ich starte kurz neu! Bitte gib mir einen kleinen moment. Dein Meow 1337 Bot <3");
            for (int i = 5; i >= 0; i--)
            {
                if (i > 0)
                {
                    await SendToGeneralChannelAsync("Noch " + i.ToString() + " sec.");
                    
                }
                else if (i == 0)
                {
                    await SendToGeneralChannelAsync("Bis gleich o/");
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

        public async Task SetOutputID(string args, string commandname, string modul, string modulID)
        {
            // need a argument
            if (args != null)
            {
                string[] countArgs = args.Split(' ');
                // check is exact one argument
                if (countArgs.Length != 1)
                {
                    await SendToGeneralChannelAsync("Zu viele Agumente! Bitte `" + Prefix + commandname + " <channelid>` angeben.");

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
                        await SendToGeneralChannelAsync("Die ID besteht nicht nur aus Zahlen!");
                        return;
                    }

                    // check if channel id exist
                    var allChannel = Context.Guild.Channels;
                    bool exist = false;
                    foreach (var item in allChannel)
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
                            await SendToGeneralChannelAsync("Der Channel wurde auf Standardeinstellung gesetzt.");
                            await SendToGeneralChannelAsync("Ich Lese und Antworte jetzt immer im selben Channel.");
                        }
                        else
                        {
                            await SendToGeneralChannelAsync("Der Channel `" + Context.Guild.GetChannel(ulongID).Name +
                            "` wurde für das Modul `"+ modul +"` gesetzt.");
                        }
                    }
                    else
                    {
                        await SendToGeneralChannelAsync("Der Channel existiert nicht.");
                    }
                }
            }
            else
            {
                await SendToGeneralChannelAsync("Du hast zu wenig Argumente angebene. Bitte nutze den Befehl wie folgt: `" + Prefix + commandname + " <channelid>`");
            }
        }
    }
}
