using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lilac_x3_Bot
{
    class Program
    {
        // Member
        private DiscordSocketClient _client;
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

            //await _client.LoginAsync(TokenType.Bot, "Token Here");
            //await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
