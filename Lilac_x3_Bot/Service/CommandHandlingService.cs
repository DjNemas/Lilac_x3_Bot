using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.Commands;
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
        private CommandHeader _header;
        private bool _devMode;
        private char _prefix = ' ';

        public CommandHandlingService()
        {
            ConfigXML configXML = new ConfigXML();
            var config = configXML.LoadConfigXML();
            var prefixQuery = from pre in config.Descendants("General")
                              select pre;

            foreach (var item in prefixQuery)
            {
                this._prefix = item.Element("Prefix").Value[0];
            }
        }

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

            this._header = new CommandHeader();
            this._commandsWithPrivilegs = CompleteCommandListWithPropertys();

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
                    currentCommand.module = _commandsWithPrivilegs[i].module;
                }
            }

            if (result.Error.HasValue && currentCommand.prefix == splitedMsg[0])
            {
                if (currentCommand.module == Module.General)
                {
                    bool check = _header.ReadChannelGeneral(context);
                    if (!check) return;

                    await this._header.SendToGeneralChannelAsync(
                    "Du hast nicht die Berechtigung `" + currentCommand.prefix +
                    "` zu nutzen, du brauchst mindestens `" + currentCommand.privilegName + "` Rechte."
                    , context);
                }
                else if (currentCommand.module == Module.Feature1337)
                {
                    bool check = _header.ReadChannel1337(context);
                    if (!check) return;

                    await this._header.SendTo1337ChannelAsync(
                    "Du hast nicht die Berechtigung `" + currentCommand.prefix +
                    "` zu nutzen, du brauchst mindestens `" + currentCommand.privilegName + "` Rechte."
                    , context);
                }

            }
            else if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
            {
                await this._header.SendToGeneralChannelAsync(result.ToString(), context);
            }
        }

        public void AddAbonents(Func<SocketMessage,Task> AddMessageReceived)
        {
            this._client.MessageReceived += AddMessageReceived;
        }

        public List<CommandsWithPrivilegs> CompleteCommandListWithPropertys()
        {
            List<CommandsWithPrivilegs> commandsWithPrivilegs = new List<CommandsWithPrivilegs>();
            // Add Prefix Command
            var prefix = new CommandsWithPrivilegs();
            prefix.prefix = "!prefix";
            prefix.privileg = Privilegs.ServerAdministrator;
            prefix.privilegName = "Serverweit Administrator";
            prefix.module = Module.General;
            commandsWithPrivilegs.Add(prefix);
            // Add Shutdown Command
            var shutdown = new CommandsWithPrivilegs();
            shutdown.prefix = "!shutdown";
            shutdown.privileg = Privilegs.ServerAdministrator;
            shutdown.privilegName = "Serverweit Administrator";
            shutdown.module = Module.General;
            commandsWithPrivilegs.Add(shutdown);
            // Add Restart Command
            var restart = new CommandsWithPrivilegs();
            restart.prefix = "!restart";
            restart.privileg = Privilegs.ServerAdministrator;
            restart.privilegName = "Serverweit Administrator";
            restart.module = Module.General;
            commandsWithPrivilegs.Add(restart);
            // Add setoutputchannelgeneral Command
            var setoutputchannelgeneral = new CommandsWithPrivilegs();
            setoutputchannelgeneral.prefix = "!setoutputchannelgeneral";
            setoutputchannelgeneral.privileg = Privilegs.ServerAdministrator;
            setoutputchannelgeneral.privilegName = "Serverweit Administrator";
            setoutputchannelgeneral.module = Module.General;
            commandsWithPrivilegs.Add(setoutputchannelgeneral);
            // Add setoutputchannel1337 Command
            var setoutputchannel1337 = new CommandsWithPrivilegs();
            setoutputchannel1337.prefix = "!setoutputchannel1337";
            setoutputchannel1337.privileg = Privilegs.ServerAdministrator;
            setoutputchannel1337.privilegName = "Serverweit Administrator";
            setoutputchannel1337.module = Module.General;
            commandsWithPrivilegs.Add(setoutputchannel1337);
            // Add setinputchannelgeneral Command
            var setinputchannelgeneral = new CommandsWithPrivilegs();
            setinputchannelgeneral.prefix = "!setinputchannelgeneral";
            setinputchannelgeneral.privileg = Privilegs.ServerAdministrator;
            setinputchannelgeneral.privilegName = "Serverweit Administrator";
            setinputchannelgeneral.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannelgeneral);
            // Add setinputchannel1337commands Command
            var setinputchannel1337commands = new CommandsWithPrivilegs();
            setinputchannel1337commands.prefix = "!setinputchannel1337commands";
            setinputchannel1337commands.privileg = Privilegs.ServerAdministrator;
            setinputchannel1337commands.privilegName = "Serverweit Administrator";
            setinputchannel1337commands.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannel1337commands);
            // Add setinputchannel1337listen Command
            var setinputchannel1337listen = new CommandsWithPrivilegs();
            setinputchannel1337listen.prefix = "!setinputchannel1337listen";
            setinputchannel1337listen.privileg = Privilegs.ServerAdministrator;
            setinputchannel1337listen.privilegName = "Serverweit Administrator";
            setinputchannel1337listen.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannel1337listen);
            // Add setinputchannel1337commands Command
            var test1337 = new CommandsWithPrivilegs();
            test1337.prefix = "!test1337";
            test1337.privileg = Privilegs.ChannelManageChannel;
            test1337.privilegName = "Channelweit Kanal verwalten";
            test1337.module = Module.Feature1337;
            commandsWithPrivilegs.Add(test1337);
            var testgeneral = new CommandsWithPrivilegs();
            testgeneral.prefix = "!testgeneral";
            testgeneral.privileg = Privilegs.ChannelManageChannel;
            testgeneral.privilegName = "Channelweit Kanal verwalten";
            testgeneral.module = Module.General;
            commandsWithPrivilegs.Add(testgeneral);

            return commandsWithPrivilegs;
        }

        public struct CommandsWithPrivilegs
        {
            public string prefix;
            public Privilegs privileg;
            public string privilegName;
            public Module module;
        }

        public enum Privilegs
        {
            ServerAdministrator,
            ChannelManageChannel
        }

        public enum Module
        {
            General,
            Feature1337
        }

    }
}
