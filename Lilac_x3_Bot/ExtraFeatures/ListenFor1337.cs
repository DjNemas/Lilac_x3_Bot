using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.Service;
using System.Threading.Tasks;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Database.Tables;
using System.Data.SQLite;
using System.Data.Linq;
using System;
using System.Linq;

namespace Lilac_x3_Bot.ExtraFeatures
{
    class ListenFor1337
    {
        // Member
        DiscordSocketClient _client;
        CommandHandlingService _command;
        DatabaseInit dbClass = new DatabaseInit();
        SQLiteConnection db;

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

            db = dbClass.connect();
            TablesHeader dbCommand = new TablesHeader(db);


            /// BEISPIEL für mich
            //Table1337 table = new Table1337
            //{
            //    userid = 1,
            //    username = "test"
            //};

            //await Task.Run( () =>
            //{
            //    dbCommand.Table1337.InsertOnSubmit(table);
            //    dbCommand.SubmitChanges();
            //});
            

            //var table2 = dbCommand.Table1337;

            //foreach (var item in table2)
            //{
            //    Console.WriteLine("UserID: " + item.userid + " UserName: " + item.username); 
            //}



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
                var table1337 = dbCommand.Table1337;
                var userID = from table in table1337
                             where table.userid == message.Author.Id
                             select table.userid;

                if (userID.Count() == 0)
                {
                    Table1337 addNewUser = new Table1337()
                    {
                        userid = message.Author.Id,
                        username = context.Guild.GetUser(message.Author.Id).Nickname,
                        counter_all = 1,
                        counter_streak = 1,
                        counter_longest_streak = 1,
                        date_begin = DateTime.Today.ToShortDateString(),
                        date_last = DateTime.Today.ToShortDateString()
                    };

                    dbCommand.Table1337.InsertOnSubmit(addNewUser);
                    dbCommand.SubmitChanges();
                }
                else
                {

                    var userID2 = from table in table1337
                                  where table.userid == message.Author.Id
                                  select table;

                    int counter_allQ = 0;
                    int counter_streakQ = 0;
                    int counter_logest_streakQ = 0;
                    string date_lastQ = " ";

                    foreach (var item in userID2)
                    {
                        counter_allQ = item.counter_all;
                        counter_streakQ = item.counter_all;
                        counter_logest_streakQ = item.counter_longest_streak;
                        date_lastQ = item.date_last;
                    }

                    if (counter_streakQ > counter_logest_streakQ)
                    {
                        counter_logest_streakQ = counter_streakQ;
                    }

                    foreach (var item in table1337)
                    {
                        item.username = context.Guild.GetUser(message.Author.Id).Nickname;
                        item.counter_all = counter_allQ++;
                        item.counter_streak = counter_streakQ++; // Muss noch angepasst werden
                        item.counter_longest_streak = counter_logest_streakQ;
                        item.date_last = DateTime.Today.ToShortDateString();
                    }
                    dbCommand.SubmitChanges();
                }
                Console.WriteLine("Check");
                db.Close();
                await context.Channel.SendMessageAsync(context.Guild.GetUser(message.Author.Id).Mention + " Hab dich gezählt :P");
            }
            
            //await context.Guild.GetTextChannel(325653182736760832).SendMessageAsync(context.Guild.GetUser(message.Author.Id).Nickname + " dein text `" + message + "` wurde gezählt.");
            //await context.Channel.SendMessageAsync(context.Guild.GetUser(message.Author.Id).Nickname + " dein text `" + message + "` wurde gezählt.");


        }
    }
}
