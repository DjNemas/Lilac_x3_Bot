using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    
    public class CommandsAll : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        {
            
            await ReplyAsync(echo);
        }

        [Command("test")]
        [Summary("Write in Console.")]
        public async Task TestAsync()
        {
            Console.WriteLine("Channel: " + Context.Channel + "\n");
            Console.WriteLine("Client: " + Context.Client + "\n");
            Console.WriteLine("Guild: " + Context.Guild + "\n");
            Console.WriteLine("Message: " + Context.Message + "\n");
            Console.WriteLine("User: " + Context.User + "\n");

        }

    }
}
