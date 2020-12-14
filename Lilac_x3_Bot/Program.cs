using Discord;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lilac_x3_Bot
{
    class Program
    {
        // Member
        private DiscordSocketClient _client;
        private Tools t = new Tools();
        private ConfigXML configClass = new ConfigXML();
        private XDocument configXML;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
            

        public async Task MainAsync()
        {

            // Load config.xml if doesn't exist create one. Both with exception handling.
            this.configXML = this.configClass.LoadConfigXML();
            
            this._client = new DiscordSocketClient();

            this._client.Log += Log;
            if (this.configXML != null)
            {
                await _client.LoginAsync(TokenType.Bot, this.configClass.GetToken(configXML));
            }
            else
            {
                CloseByInvalidToken();
            }
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private void CloseByInvalidToken()
        {
            t.CWLTextColor("Der Token ist Invalid, bitte überprüfe den Token!", ConsoleColor.Red);
            t.CWLTextColor("Das Programm wird in 5 sec geschlossen!", ConsoleColor.Yellow);
            for (int i = 5; i >= 0; i--)
            {
                if (i > 0)
                {
                    t.CWLTextColor(i.ToString(), ConsoleColor.Yellow);
                }
                else if (i == 0)
                {
                    t.CWLTextColor("Programm is closing", ConsoleColor.Yellow);
                }
                Thread.Sleep(1000);
            }
            System.Environment.Exit(1);
        }
    }
}
