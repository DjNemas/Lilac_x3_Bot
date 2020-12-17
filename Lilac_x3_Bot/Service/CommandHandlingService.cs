using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lilac_x3_Bot.Service
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private IServiceProvider _service;
        private bool _devMode;
        private char _prefix = ' ';

        public CommandHandlingService(DiscordSocketClient client, CommandService commands, XDocument configXML, bool devMode)
        {
            this._client = client;
            this._commands = commands;
            this._devMode = devMode;

            var prefixQuery = from pre in configXML.Descendants("General")
                            select pre;

            foreach (var item in prefixQuery)
            {
                this._prefix = item.Element("Prefix").Value[0];
            }


            this._client.MessageReceived += MessageReceived;
        }

        public char GetPrefix()
        {
            return this._prefix;
        }

        public async Task InitializeAsync(IServiceProvider service)
        {
            this._service = service;
            await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            // Add additional initialization code here...
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            Console.WriteLine("Prefix: " + this._prefix);
            int argPos = 0;
            if (!message.HasCharPrefix(this._prefix, ref argPos)) return;

            var context = new SocketCommandContext(_client, message);
            var result = await this._commands.ExecuteAsync(context, argPos, _service);

            if (result.Error.HasValue &&
                result.Error.Value != CommandError.UnknownCommand && _devMode)
                await context.Channel.SendMessageAsync(result.ToString());
        }

        public void AddAbonents(Func<SocketMessage,Task> AddMessageReceived)
        {
            this._client.MessageReceived += AddMessageReceived;
        }
    }
}
