using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
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
        private IReadOnlyCollection<SocketGuildUser> users;
        private ulong channelIDRead = 0;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
            

        public async Task MainAsync()
        {

            // Load config.xml if doesn't exist create one. Both with exception handling.
            this.configXML = this.configClass.LoadConfigXML();

            this._client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 200,
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true,
                LargeThreshold = 250
            });

            this._client.Log += Log;
            this._client.MessageReceived += MessageReceived;


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

        private async Task MessageReceived(SocketMessage message)
        {
            Console.WriteLine(message.Author.Id);

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
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
