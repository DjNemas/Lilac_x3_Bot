using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
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
        private List<CommandsWithPrivilegs> _commandsWithPrivilegs;
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

            this._commandsWithPrivilegs = CreateCommandsWithPrivilegs();

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

            if (_devMode) Console.WriteLine("Prefix: " + this._prefix);
            int argPos = 0;
            if (!message.HasCharPrefix(this._prefix, ref argPos)) return;
            
            var context = new SocketCommandContext(_client, message);
            var result = await this._commands.ExecuteAsync(context, argPos, _service);

            var splitedMsg = message.Content.Split(" ");
            CommandsWithPrivilegs currentCommand = new CommandsWithPrivilegs();
            for (int i = 0; i < _commandsWithPrivilegs.Count; i++)
            {
                if (splitedMsg[0] == _commandsWithPrivilegs[i].prefix)
                {
                    currentCommand.prefix = _commandsWithPrivilegs[i].prefix;
                    currentCommand.privileg = _commandsWithPrivilegs[i].privileg;
                    currentCommand.privilegName = _commandsWithPrivilegs[i].privilegName;
                }
            }

            if (result.Error.HasValue && currentCommand.prefix == splitedMsg[0])
            {
                await context.Channel.SendMessageAsync("Du hast nicht die Berechtigung `" + currentCommand.prefix +
                    "` zu nutzen, du brauchst mindestens `" + currentCommand.privilegName + "` Rechte.");
            }
            else if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
            {
                await context.Channel.SendMessageAsync(result.ToString());
            }
        }

        public void AddAbonents(Func<SocketMessage,Task> AddMessageReceived)
        {
            this._client.MessageReceived += AddMessageReceived;
        }

        public List<CommandsWithPrivilegs> CreateCommandsWithPrivilegs()
        {
            List<CommandsWithPrivilegs> commandsWithPrivilegs = new List<CommandsWithPrivilegs>();
            // Add Prefix Command
            var prefix = new CommandsWithPrivilegs();            
            prefix.prefix = "!prefix";
            prefix.privileg = Privilegs.Administrator;
            prefix.privilegName = "Administrator";
            commandsWithPrivilegs.Add(prefix);
            // Add Shutdown Command
            var shutdown = new CommandsWithPrivilegs();
            shutdown.prefix = "!shutdown";
            shutdown.privileg = Privilegs.Administrator;
            shutdown.privilegName = "Administrator";
            commandsWithPrivilegs.Add(shutdown);
            // Add Restart Command
            var restart = new CommandsWithPrivilegs();
            restart.prefix = "!restart";
            restart.privileg = Privilegs.Administrator;
            restart.privilegName = "Administrator";
            commandsWithPrivilegs.Add(restart);
            // Add Meow Command
            var meow = new CommandsWithPrivilegs();
            meow.prefix = "!meow";
            meow.privileg = Privilegs.ManageChannel;
            meow.privilegName = "Kanal verwalten";
            commandsWithPrivilegs.Add(meow);

            return commandsWithPrivilegs;
        }

        public struct CommandsWithPrivilegs
        {
            public string prefix;
            public Privilegs privileg;
            public string privilegName;
        }

        public enum Privilegs
        {
            Administrator,
            ManageChannel
        }

    }
}
