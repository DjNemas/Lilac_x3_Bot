using Discord.Commands;
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
        public static ulong Feature1337WriteIntoChannelID { get; set; }
        public static ulong Feature1337ReadFromChannelID { get; set; }
        public static ulong GenerelWriteIntoChannelID { get; set; }
        public static ulong GenerelReadFromChannelID { get; set; }

        public CommandHeader()
        {
            Feature1337WriteIntoChannelID = this._configXML.GetChannelUID("Feature1337","WriteIntoChannel");
            Feature1337ReadFromChannelID = this._configXML.GetChannelUID("Feature1337", "ReadFromChannel");
            GenerelWriteIntoChannelID = this._configXML.GetChannelUID("General", "WriteIntoChannel");
            GenerelReadFromChannelID = this._configXML.GetChannelUID("General", "ReadFromChannel");
        }

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
    }
}
