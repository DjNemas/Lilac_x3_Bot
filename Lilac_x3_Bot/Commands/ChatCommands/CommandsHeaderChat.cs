using Discord;
using Discord.Commands;
using Lilac_x3_Bot.Commands.Functions;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsHeaderChat : ModuleBase<SocketCommandContext> 
    {
        protected ContextInfo ContextInfo;
        protected CommandsAdmin Admin;
        protected CommandsAll All;

        protected void LoadCommandsHeaderChat()
        {
            if (Context != null)
            {
                ContextInfo = new ContextInfo(Context.Guild.Id, Context.Channel.Id, Context.User.Id);
                Admin = new CommandsAdmin(Context.Client, ContextInfo);
                All = new CommandsAll(Context.Client, ContextInfo);
            }
        }

    }
}
