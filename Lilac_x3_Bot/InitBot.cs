using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.ExtraFeatures;
using Lilac_x3_Bot.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lilac_x3_Bot
{
    class InitBot
    {
        // DevMode Member
        bool _devMode = false;
        //Member
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private Tools t = new Tools();
        private ConfigXML configClass = new ConfigXML();
        private XDocument configXML;
        public async Task MainAsync()
        {
            // New Client
            // Load config.xml if doesn't exist create one. Both with exception handling.
            this.configXML = this.configClass.LoadConfigXML();

            this._client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 200,
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true,
                LargeThreshold = 250
            });

            // New CommandService
            this._commands = new CommandService(new CommandServiceConfig
            {
                // Config Here if needed
                LogLevel = LogSeverity.Info
            });

            // New CommandHandlingService
            CommandHandlingService _cmdHandService = new CommandHandlingService(_client, _commands, configXML);

            // Add Own Features
            ListenFor1337 _listenFor1337 = new ListenFor1337(_client, _cmdHandService, _devMode);

            // New ServiceCollection
            this._services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_cmdHandService)
                // Add More Services below if needed
                .AddSingleton(_listenFor1337)
                .BuildServiceProvider();

            // Init CommandHandlingService
            await _cmdHandService.InitializeAsync(_services);

            this._client.Log += Log;
            this._client.UserJoined += UserJoined;

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

        private Task UserJoined(SocketGuildUser user)
        {
            t.CWLTextColor("User: " + user.Username + "Joined the Guild.", ConsoleColor.Yellow);
            Task.Run(async () =>
            {
                await _client.DownloadUsersAsync(_client.Guilds);
            });           
            t.CWLTextColor("All Users downloaded", ConsoleColor.Yellow);
            return Task.CompletedTask;
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
