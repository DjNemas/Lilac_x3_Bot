using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.ExtraFeatures
{
    class ListenFor1337
    {
        // Member
        DiscordSocketClient _client;
        CommandHandlingService _command;        

        public ListenFor1337(DiscordSocketClient client, CommandHandlingService command)
        {
            _client = client;
            _command = command;
            _command.AddAbonents(Message1337);
        }

        private async Task Message1337(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            var context = new SocketCommandContext(_client, message);

            string rolles = " ";
            if (message.MentionedRoles.Count == 1)
            {
                foreach (var item in message.MentionedRoles)
                {
                    if (item.Name == "1337")
                    {
                        rolles = item.Id.ToString();
                    }                    
                }
            }

            if (message.Content == "1337" || message.Content == "<@&" + rolles + ">")
            {
                await context.Channel.SendMessageAsync("Jep Klappt");
            }
            
            //await context.Guild.GetTextChannel(325653182736760832).SendMessageAsync(context.Guild.GetUser(message.Author.Id).Nickname + " dein text `" + message + "` wurde gezählt.");
            //await context.Channel.SendMessageAsync(context.Guild.GetUser(message.Author.Id).Nickname + " dein text `" + message + "` wurde gezählt.");


        }
    }
}
