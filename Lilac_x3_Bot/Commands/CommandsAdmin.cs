﻿using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsAdmin : CommandHeader
    {
        [Command("commands")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CommandsGeneralAdminAsync([Remainder] string args = null)
        {
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            if (args == null)
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + "\n>>> Nutze den Befehl wie folgt: `" +
                    this.Prefix + "commands <Modul> <Berechtigungsgruppe>`\nModule können mit `" + this.Prefix + "moduls` angezeigt werden.");
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length < 2)
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu wenige Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
            }
            else if (countArgs.Length > 2)
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente! Bitte `" + Prefix + "commands <Modul> <Berechtigungsgruppe>` angeben.");
            }
            else
            {
                if (countArgs[0] != "general" && countArgs[0] != "1337")
                {
                    await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Falsches Modul\nBitte nutze " +
                        this.Prefix + "moduls um eine Liste der Module zu erhalten.");
                    return;
                }
                if (countArgs[1] != "all" && countArgs[1] != "admin" && countArgs[1] != "mod")
                {
                    await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Falsche Berechtigungsgruppe\nBitte nutze " +
                        this.Prefix + "moduls um eine Liste der Berechtigungsgruppen zu erhalten.");
                    return;
                }
                if (countArgs[0] == "general" && countArgs[1] == "all")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralAllCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
                if (countArgs[0] == "general" && countArgs[1] == "mod")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralModCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
                if (countArgs[0] == "general" && countArgs[1] == "admin")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = GeneralAdminCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
                if (countArgs[0] == "1337" && countArgs[1] == "all")
                {
                    var str = new StringBuilder();
                    str = HeaderCommandsList(str);
                    str = Feature1337AllCommandsList(str);
                    await this.SendToGeneralChannelAdminAsync(str.ToString());
                }
            }
        }

        [Command("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PrefixAsyc([Remainder] string args = null)
        {
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            if (args != null)
            {
                string[] countArgs = args.Split(' ');

                if (countArgs.Length != 1)
                {
                    await this.SendToGeneralChannelAdminAsync("Zu viele Agumente! Bitte `"+ Prefix + "prefix <prefix>` angeben.");

                }
                else if (countArgs[0].Length > 1)
                {
                    await this.SendToGeneralChannelAdminAsync("Der Prefix ist zu lang. Nur ein Zeichen!");
                }
                else
                {
                    ConfigXML config = new ConfigXML();
                    config.ChangePrefix(countArgs[0][0]);
                    await this.SendToGeneralChannelAdminAsync("Der Prefix wurde zu `" + countArgs[0][0] + "` geändert!");
                    await this.SendToGeneralChannelAdminAsync("> Info: Der Befehl brauch ein `" + this.Prefix + "restart`");
                }
            }
            else
            {
                await this.SendToGeneralChannelAdminAsync("Du hast zu wenig Argumente angebene. Bitte nutze den Befehl wie folgt: `" + Prefix + "prefix <prefix>`");
            }
        }

        [Command("restart")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RestartAsyc()
        {
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.RestartBot();
        }

        [Command("setoutputchannelgeneralall")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetOutputChannelGeneralAllAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.SetOutputID(args, "setoutputchannelgeneralall", "General", "All", "WriteIntoChannelAll");
        }
        [Command("setinputchannelgeneralall")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannelGeneralAllAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.SetOutputID(args, "setinputchannelgeneralall", "General", "All", "ReadFromChannelAll");
        }

        [Command("setoutputchannelgeneraladmin")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetOutputChannelGeneralAdminAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.SetOutputID(args, "setoutputchannelgeneraladmin", "General", "Admin", "WriteIntoChannelAdmin");
        }
        [Command("setinputchannelgeneraladmin")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannelGeneralAdminAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.SetOutputID(args, "setinputchannelgeneraladmin", "General", "Admin", "ReadFromChannelAdmin");
        }

        [Command("setoutputchannel1337")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetOutputChannel1337Async([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.SetOutputID(args, "setoutputchannel1337", "Feature1337", "All", "WriteIntoChannel");
        }

        [Command("setinputchannel1337commands")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannel1337ReadAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;

            await this.SetOutputID(args, "setinputchannel1337commands", "Feature1337", "All", "ReadFromChannel");
        }

        [Command("setinputchannel1337listen")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetInputChannel1337ListenAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;
            await this.SetOutputID(args, "setinputchannel1337read", "Feature1337", "Listen", "Listen1337FromChannel");
        }

        [Command("setmodroleid")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetModRoleIDAsync([Remainder] string args = null)
        {
            // return if its not the Modul Channel ID or 0
            bool check = this.ReadChannelGeneralAdmin();
            if (!check) return;
            await this.SetOutputID(args, "setmodroleid", "General", "Admin", "ModRoleID");
        }
    }
}
