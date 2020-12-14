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
        private Tools t = new Tools();
        private XDocument configXML;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
            

        public async Task MainAsync()
        {
            // Load config.xml if doesn't exist create one. Both with exception handling.
            this.configXML = this.LoadConfigXML();

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

        #region Load and Create Config Methodes
        private XDocument LoadConfigXML()
        {
            try
            {
                XDocument configXML = XDocument.Load("config.xml");
                return configXML;
            }
            catch (Exception)
            {
                this.CreateConfigXML();
                return null;
            }
        }

        private void CreateConfigXML()
        {
            try
            {
                XDocument config = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                    new XElement("Init",
                                                        new XElement("Token", "Token Here")
                                                    )
                                                );
                config.Save("config.xml");
                Console.WriteLine("config.xml war nicht vorhanden und wurde erstellt.");
                this.t.CWLTextColor("Bitte die config.xml anpassen!", ConsoleColor.Yellow);
            }
            catch (Exception e)
            {
                this.t.CWLTextColor("config.xml konnte nicht erstellt werden.\nMore Details:", ConsoleColor.Red);
                Console.WriteLine(e);
            }
        }
        #endregion
    }
}
