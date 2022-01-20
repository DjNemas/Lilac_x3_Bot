using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.Commands;
using Lilac_x3_Bot.Commands.Functions;
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

        public CommandHandlingService()
        {
            ConfigXML configXML = new ConfigXML();
            this._prefix = configXML.GetPrefix();
        }

        public CommandHandlingService(DiscordSocketClient client, CommandService commands, bool devMode)
        {
            this._client = client;
            this._commands = commands;
            this._devMode = devMode;

            ConfigXML configXML = new ConfigXML();
            this._prefix = configXML.GetPrefix();

            this._commandsWithPrivilegs = CompleteCommandListWithPropertys();

            this._client.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider service)
        {
            this._service = service;
            await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            // Add additional initialization code here...
        }

        public async Task MessageReceived(SocketMessage rawMessage)
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
            await ContextResultHandler(result, currentCommand, context, splitedMsg);
        }

        public async Task ContextResultHandler(IResult result, CommandsWithPrivilegs currentCommand, SocketCommandContext context, string[] splitedMsg)
        {

            CommandsHeader comHeader = new CommandsHeader(context.Client, new ContextInfo(context.Guild.Id, context.Channel.Id, context.User.Id));

            if (result.Error.HasValue && currentCommand.prefix == splitedMsg[0] && result.Error == CommandError.UnmetPrecondition)
            {
                if (currentCommand.module == Module.General)
                {
                    if (currentCommand.privileg == Privilegs.All)
                    {
                        bool check = comHeader.ReadChannelGeneralAll();
                        if (!check) return;

                        await comHeader.SendToGeneralChannelAllAsync(
                        "Du hast nicht die Berechtigung `" + currentCommand.prefix +
                        "` zu nutzen, du brauchst mindestens `" + currentCommand.privilegName + "` Rechte.");

                    }
                    if (currentCommand.privileg == Privilegs.ServerAdministrator || currentCommand.privileg == Privilegs.ModRole)
                    {
                        bool check = comHeader.ReadChannelGeneralAdmin();
                        if (!check) return;

                        await comHeader.SendToGeneralChannelAdminAsync(
                        "Du hast nicht die Berechtigung `" + currentCommand.prefix +
                        "` zu nutzen, du brauchst mindestens `" + currentCommand.privilegName + "` Rechte.");
                    }
                }
                else if (currentCommand.module == Module.Feature1337)
                {
                    bool check = comHeader.ReadChannel1337();
                    if (!check) return;

                    await comHeader.SendTo1337ChannelAsync(
                    "Du hast nicht die Berechtigung `" + currentCommand.prefix +
                    "` zu nutzen, du brauchst mindestens `" + currentCommand.privilegName + "` Rechte.");
                }
            }
            else if (result.Error.HasValue && result.Error.Value == CommandError.UnknownCommand)
            {
                bool check = false;
                if (comHeader.ReadChannelGeneralAll() ||
                    comHeader.ReadChannelGeneralAdmin() ||
                    comHeader.ReadChannel1337())
                    check = true;

                if (!check) return;

                // Check user has mod role
                var userRoles = context.User as SocketGuildUser;
                bool userHasModRole = false;
                Console.WriteLine(CommandsHeader.ModRoleID); // Checken
                foreach (var item in userRoles.Roles)
                {
                    if (item.Id == CommandsHeader.ModRoleID)
                        userHasModRole = true;
                }

                // if not admin channel and user has mod role
                if (context.Channel.Id == CommandsHeader.GenerelReadFromChannelAdminID &&
                    !(userHasModRole || userRoles.GuildPermissions.Administrator))
                    return;
                await context.Channel.SendMessageAsync("Solch ein Command kenne ich nicht. :sob:");
            }
            else if (result.Error.HasValue && result.Error.Value == CommandError.ObjectNotFound)
            {
                if (currentCommand.module == Module.General)
                {
                    if (currentCommand.privileg == Privilegs.All)
                    {
                        bool check = false;
                        if (comHeader.ReadChannelGeneralAll() ||
                            comHeader.ReadChannelGeneralAdmin() ||
                            comHeader.ReadChannel1337())
                            check = true;
                        if (!check) return;

                        await context.Channel.SendMessageAsync("Falsches Objekt übergeben. Bitte überprüfe deine Argumente noch einmal.\n`Result: " + result.ToString() + "`");

                    }
                    if (currentCommand.privileg == Privilegs.ServerAdministrator || currentCommand.privileg == Privilegs.ModRole)
                    {
                        bool check = comHeader.ReadChannelGeneralAdmin();
                        if (!check) return;

                        await context.Channel.SendMessageAsync("Falsches Objekt übergeben. Bitte überprüfe deine Argumente noch einmal.\n`Result: " + result.ToString() + "`");
                    }
                }
                else if (currentCommand.module == Module.Feature1337)
                {
                    bool check = comHeader.ReadChannel1337();
                    if (!check) return;

                    await context.Channel.SendMessageAsync("Falsches Objekt übergeben. Bitte überprüfe deine Argumente noch einmal.\n`Result: " + result.ToString() + "`");
                }
            }
            else if (result.Error.HasValue && result.Error.Value == CommandError.BadArgCount)
            {
                if (currentCommand.module == Module.General)
                {
                    if (currentCommand.privileg == Privilegs.All)
                    {
                        bool check = false;
                        if (comHeader.ReadChannelGeneralAll() ||
                            comHeader.ReadChannelGeneralAdmin() ||
                            comHeader.ReadChannel1337())
                            check = true;
                        if (!check) return;

                        await context.Channel.SendMessageAsync("Zu viele oder zu wenige Argumente angegeben.\n`Result: " + result.ToString() + "`");

                    }
                    if (currentCommand.privileg == Privilegs.ServerAdministrator || currentCommand.privileg == Privilegs.ModRole)
                    {
                        bool check = comHeader.ReadChannelGeneralAdmin();
                        if (!check) return;

                        await context.Channel.SendMessageAsync("Zu viele oder zu wenige Argumente angegeben.\n`Result: " + result.ToString() + "`");
                    }
                }
                else if (currentCommand.module == Module.Feature1337)
                {
                    bool check = comHeader.ReadChannel1337();
                    if (!check) return;

                    await context.Channel.SendMessageAsync("Zu viele oder zu wenige Argumente angegeben.\n`Result: " + result.ToString() + "`");
                }
            }
            else if (result.Error.HasValue)
            {
                bool check = false;
                if (comHeader.ReadChannelGeneralAll() ||
                    comHeader.ReadChannelGeneralAdmin() ||
                    comHeader.ReadChannel1337())
                    check = true;

                if (!check) return;

                // Check user has mod role
                var userRoles = context.User as SocketGuildUser;
                bool userHasModRole = false;
                foreach (var item in userRoles.Roles)
                {
                    if (item.Id == CommandsHeader.ModRoleID)
                        userHasModRole = true;
                }

                // if not admin channel and user has mod role
                if (context.Channel.Id == CommandsHeader.GenerelReadFromChannelAdminID &&
                    !(userHasModRole || userRoles.GuildPermissions.Administrator))
                    return;

                await context.Channel.SendMessageAsync("Every Exeption that is not Handled by Bot Developer: `" + result.ToString() + "`\n**Pls contact the Developer!**");
            }
        }

        public void AddAbonents(Func<SocketMessage, Task> AddMessageReceived)
        {
            this._client.MessageReceived += AddMessageReceived;
        }

        public List<CommandsWithPrivilegs> CompleteCommandListWithPropertys()
        {
            List<CommandsWithPrivilegs> commandsWithPrivilegs = new List<CommandsWithPrivilegs>();

            // Add Prefix Command
            var prefix = new CommandsWithPrivilegs();
            prefix.prefix = this._prefix + "prefix";
            prefix.privileg = Privilegs.ServerAdministrator;
            prefix.privilegName = "Serverweit Administrator";
            prefix.module = Module.General;
            commandsWithPrivilegs.Add(prefix);

            // Add Restart Command
            var restart = new CommandsWithPrivilegs();
            restart.prefix = this._prefix + "restart";
            restart.privileg = Privilegs.ServerAdministrator;
            restart.privilegName = "Serverweit Administrator";
            restart.module = Module.General;
            commandsWithPrivilegs.Add(restart);

            // Add Moduls Command
            var moduls = new CommandsWithPrivilegs();
            moduls.prefix = this._prefix + "moduls";
            moduls.privileg = Privilegs.All;
            moduls.privilegName = "All";
            moduls.module = Module.General;
            commandsWithPrivilegs.Add(moduls);

            // Add Commands Command
            var commands = new CommandsWithPrivilegs();
            commands.prefix = this._prefix + "commands";
            commands.privileg = Privilegs.All;
            commands.privilegName = "All";
            commands.module = Module.General;
            commandsWithPrivilegs.Add(commands);

            // Add Credits Command
            var credits = new CommandsWithPrivilegs();
            credits.prefix = this._prefix + "credits";
            credits.privileg = Privilegs.All;
            credits.privilegName = "All";
            credits.module = Module.General;
            commandsWithPrivilegs.Add(credits);

            // Add Version Command
            var version = new CommandsWithPrivilegs();
            version.prefix = this._prefix + "version";
            version.privileg = Privilegs.All;
            version.privilegName = "All";
            version.module = Module.General;
            commandsWithPrivilegs.Add(version);

            // Add Rollback Command
            var rollback = new CommandsWithPrivilegs();
            rollback.prefix = this._prefix + "rollback";
            rollback.privileg = Privilegs.ModRole;
            rollback.privilegName = "ModRole";
            rollback.module = Module.General;
            commandsWithPrivilegs.Add(rollback);

            // Add CountUser Command
            var countuser = new CommandsWithPrivilegs();
            countuser.prefix = this._prefix + "countuser";
            countuser.privileg = Privilegs.ModRole;
            countuser.privilegName = "ModRole";
            countuser.module = Module.General;
            commandsWithPrivilegs.Add(countuser);

            // Add EditUser Command
            var edituser = new CommandsWithPrivilegs();
            edituser.prefix = this._prefix + "edituser";
            edituser.privileg = Privilegs.ModRole;
            edituser.privilegName = "ModRole";
            edituser.module = Module.General;
            commandsWithPrivilegs.Add(edituser);

            // Add ShowUserToday Command
            var showusertoday = new CommandsWithPrivilegs();
            showusertoday.prefix = this._prefix + "showusertoday";
            showusertoday.privileg = Privilegs.ModRole;
            showusertoday.privilegName = "ModRole";
            showusertoday.module = Module.General;
            commandsWithPrivilegs.Add(showusertoday);

            // Add ShowUserYesterday Command
            var showuseryesterday = new CommandsWithPrivilegs();
            showuseryesterday.prefix = this._prefix + "showuseryesterday";
            showuseryesterday.privileg = Privilegs.ModRole;
            showuseryesterday.privilegName = "ModRole";
            showuseryesterday.module = Module.General;
            commandsWithPrivilegs.Add(showuseryesterday);

            // Add ModRoleID Command
            var setmodroleid = new CommandsWithPrivilegs();
            setmodroleid.prefix = this._prefix + "setmodroleid";
            setmodroleid.privileg = Privilegs.ServerAdministrator;
            setmodroleid.privilegName = "Serverweit Administrator";
            setmodroleid.module = Module.General;
            commandsWithPrivilegs.Add(setmodroleid);

            // Add setoutputchannelgeneralall Command
            var setoutputchannelgeneralall = new CommandsWithPrivilegs();
            setoutputchannelgeneralall.prefix = this._prefix + "setoutputchannelgeneralall";
            setoutputchannelgeneralall.privileg = Privilegs.ServerAdministrator;
            setoutputchannelgeneralall.privilegName = "Serverweit Administrator";
            setoutputchannelgeneralall.module = Module.General;
            commandsWithPrivilegs.Add(setoutputchannelgeneralall);

            // Add setinputchannelgeneralall Command
            var setinputchannelgeneralall = new CommandsWithPrivilegs();
            setinputchannelgeneralall.prefix = this._prefix + "setinputchannelgeneralall";
            setinputchannelgeneralall.privileg = Privilegs.ServerAdministrator;
            setinputchannelgeneralall.privilegName = "Serverweit Administrator";
            setinputchannelgeneralall.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannelgeneralall);

            // Add setoutputchannelgeneraladmin Command
            var setoutputchannelgeneraladmin = new CommandsWithPrivilegs();
            setoutputchannelgeneraladmin.prefix = this._prefix + "setoutputchannelgeneraladmin";
            setoutputchannelgeneraladmin.privileg = Privilegs.ServerAdministrator;
            setoutputchannelgeneraladmin.privilegName = "Serverweit Administrator";
            setoutputchannelgeneraladmin.module = Module.General;
            commandsWithPrivilegs.Add(setoutputchannelgeneraladmin);

            // Add setinputchannelgeneraladmin Command
            var setinputchannelgeneraladmin = new CommandsWithPrivilegs();
            setinputchannelgeneraladmin.prefix = this._prefix + "setinputchannelgeneraladmin";
            setinputchannelgeneraladmin.privileg = Privilegs.ServerAdministrator;
            setinputchannelgeneraladmin.privilegName = "Serverweit Administrator";
            setinputchannelgeneraladmin.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannelgeneraladmin);

            // Add setoutputchannel1337 Command
            var setoutputchannel1337 = new CommandsWithPrivilegs();
            setoutputchannel1337.prefix = this._prefix + "setoutputchannel1337";
            setoutputchannel1337.privileg = Privilegs.ServerAdministrator;
            setoutputchannel1337.privilegName = "Serverweit Administrator";
            setoutputchannel1337.module = Module.General;
            commandsWithPrivilegs.Add(setoutputchannel1337);

            // Add setinputchannel1337commands Command
            var setinputchannel1337commands = new CommandsWithPrivilegs();
            setinputchannel1337commands.prefix = this._prefix + "setinputchannel1337commands";
            setinputchannel1337commands.privileg = Privilegs.ServerAdministrator;
            setinputchannel1337commands.privilegName = "Serverweit Administrator";
            setinputchannel1337commands.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannel1337commands);

            // Add setinputchannel1337listen Command
            var setinputchannel1337listen = new CommandsWithPrivilegs();
            setinputchannel1337listen.prefix = this._prefix + "setinputchannel1337listen";
            setinputchannel1337listen.privileg = Privilegs.ServerAdministrator;
            setinputchannel1337listen.privilegName = "Serverweit Administrator";
            setinputchannel1337listen.module = Module.General;
            commandsWithPrivilegs.Add(setinputchannel1337listen);

            // Add 1337streak Command
            var feature1337streak = new CommandsWithPrivilegs();
            feature1337streak.prefix = this._prefix + "1337streak";
            feature1337streak.privileg = Privilegs.All;
            feature1337streak.privilegName = "All";
            feature1337streak.module = Module.Feature1337;
            commandsWithPrivilegs.Add(feature1337streak);

            // Add feature1337count Command
            var feature1337count = new CommandsWithPrivilegs();
            feature1337count.prefix = this._prefix + "1337count";
            feature1337count.privileg = Privilegs.All;
            feature1337count.privilegName = "All";
            feature1337count.module = Module.Feature1337;
            commandsWithPrivilegs.Add(feature1337count);

            // Add feature1337count Command
            var feature1337highscore = new CommandsWithPrivilegs();
            feature1337highscore.prefix = this._prefix + "1337highscore";
            feature1337highscore.privileg = Privilegs.All;
            feature1337highscore.privilegName = "All";
            feature1337highscore.module = Module.Feature1337;
            commandsWithPrivilegs.Add(feature1337highscore);

            // Add feature1337hstreak Command
            var feature1337hstreak = new CommandsWithPrivilegs();
            feature1337hstreak.prefix = this._prefix + "1337hstreak";
            feature1337hstreak.privileg = Privilegs.All;
            feature1337hstreak.privilegName = "All";
            feature1337hstreak.module = Module.Feature1337;
            commandsWithPrivilegs.Add(feature1337hstreak);

            // Add feature1337hstreak Command
            var feature1337hcount = new CommandsWithPrivilegs();
            feature1337hcount.prefix = this._prefix + "1337hcount";
            feature1337hcount.privileg = Privilegs.All;
            feature1337hcount.privilegName = "All";
            feature1337hcount.module = Module.Feature1337;
            commandsWithPrivilegs.Add(feature1337hcount);

            // Add feature1337hstreak Command
            var feature1337info = new CommandsWithPrivilegs();
            feature1337info.prefix = this._prefix + "1337info";
            feature1337info.privileg = Privilegs.All;
            feature1337info.privilegName = "All";
            feature1337info.module = Module.Feature1337;
            commandsWithPrivilegs.Add(feature1337info);

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
            ModRole,
            All
        }

        public enum Module
        {
            General,
            Feature1337
        }

    }
}
