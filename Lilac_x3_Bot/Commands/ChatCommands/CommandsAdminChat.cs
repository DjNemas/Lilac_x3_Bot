using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.Commands.Functions;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsAdminChat : CommandsHeaderChat
    {
        [Command("commands")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CommandsGeneralAdminChatAsync([Remainder] string args = null)
        {
            LoadCommandsHeaderChat();
            await Admin.CommandsGeneralAdminChatAsync(args);
        }

        //[Command("prefix")]
        //[SlashCommand("prefix", "Hiermit stellst du den Prefix ein (nur 1 Zeichen erlaubt).")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task PrefixAsyc([Remainder] string args = null)
        //{
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    if (args != null)
        //    {
        //        string[] countArgs = args.Split(' ');

        //        if (countArgs.Length != 1)
        //        {
        //            await this.SendToGeneralChannelAdminAsync("Zu viele Agumente! Bitte `"+ Prefix + "prefix <prefix>` angeben.");

        //        }
        //        else if (countArgs[0].Length > 1)
        //        {
        //            await this.SendToGeneralChannelAdminAsync("Der Prefix ist zu lang. Nur ein Zeichen!");
        //        }
        //        else
        //        {
        //            ConfigXML config = new ConfigXML();
        //            config.ChangePrefix(countArgs[0][0]);
        //            await this.SendToGeneralChannelAdminAsync("Der Prefix wurde zu `" + countArgs[0][0] + "` geändert!");
        //            await this.SendToGeneralChannelAdminAsync("> Info: Der Befehl brauch ein `" + this.Prefix + "restart`");
        //        }
        //    }
        //    else
        //    {
        //        await this.SendToGeneralChannelAdminAsync("Du hast zu wenig Argumente angebene. Bitte nutze den Befehl wie folgt: `" + Prefix + "prefix <prefix>`");
        //    }
        //}

        //[Command("restart")]
        //[SlashCommand("restart", "Hiermit wird der Bot neugestartet. Bitte nur verwenden, wenn spezielle Einstellungen wie Prefix geändert wurde oder bei Problemen!")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task RestartAsyc()
        //{
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.RestartBot();
        //}

        //[Command("setoutputchannelgeneralall")]
        //[SlashCommand("setoutputchannelgeneralall", "Hier soll der Bot alle `Modul General All` ausgaben senden.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetOutputChannelGeneralAllAsync(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.SetOutputID(channel.Id.ToString(), "setoutputchannelgeneralall", "General", "All", "WriteIntoChannelAll");
        //}

        //[Command("setinputchannelgeneralall")]
        //[SlashCommand("setinputchannelgeneralall", "Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General All.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetInputChannelGeneralAllAsync(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.SetOutputID(channel.Id.ToString(), "setinputchannelgeneralall", "General", "All", "ReadFromChannelAll");
        //}

        //[Command("setoutputchannelgeneraladmin")]
        //[SlashCommand("setoutputchannelgeneraladmin", "Hier soll der Bot alle `Modul General Admin` ausgaben senden.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetOutputChannelGeneralAdminAsync(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.SetOutputID(channel.Id.ToString(), "setoutputchannelgeneraladmin", "General", "Admin", "WriteIntoChannelAdmin");
        //}

        //[Command("setinputchannelgeneraladmin")]
        //[SlashCommand("setinputchannelgeneraladmin", "Der Bot hört nur in dem Channel auf alle Kommandos vom Modul General Admin.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetInputChannelGeneralAdminAsync(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.SetOutputID(channel.Id.ToString(), "setinputchannelgeneraladmin", "General", "Admin", "ReadFromChannelAdmin");
        //}

        //[Command("setoutputchannel1337")]
        //[SlashCommand("setoutputchannel1337", "Hier soll der Bot alle `Modul 1337` ausgaben senden.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetOutputChannel1337Async(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.SetOutputID(channel.Id.ToString(), "setoutputchannel1337", "Feature1337", "All", "WriteIntoChannel");
        //}

        //[Command("setinputchannel1337commands")]
        //[SlashCommand("setinputchannel1337commands", "Der Bot hört nur in dem Channel auf alle Kommandos vom Modul 1337.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetInputChannel1337ReadAsync(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;

        //    await this.SetOutputID(channel.Id.ToString(), "setinputchannel1337commands", "Feature1337", "All", "ReadFromChannel");
        //}

        //[Command("setinputchannel1337listen")]
        //[SlashCommand("setinputchannel1337listen", "In dem Channel wird nach 1337 und @1337 gelauscht und zählt nur dort mit.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetInputChannel1337ListenAsync(SocketGuildChannel channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;
        //    await this.SetOutputID(channel.Id.ToString(), "setinputchannel1337read", "Feature1337", "Listen", "Listen1337FromChannel");
        //}

        //[Command("setmodrole")]
        //[SlashCommand("setmodrole", "Hiermit wird die ModRole festgelegt, mit der Mod Commands genutzt werden können.")]
        //[Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        //[Discord.Interactions.RequireUserPermission(GuildPermission.Administrator)]
        //public async Task SetModRoleIDAsync(SocketRole channel)
        //{
        //    // return if its not the Modul Channel ID or 0
        //    bool check = this.ReadChannelGeneralAdmin();
        //    if (!check) return;
        //    await this.SetOutputID(channel.Id.ToString(), "setmodroleid", "General", "Admin", "ModRoleID");
        //    await this.SendToGeneralChannelAdminAsync("> Info: Der Befehl brauch ein `" + this.Prefix + "restart`");
        //}
    }
}
