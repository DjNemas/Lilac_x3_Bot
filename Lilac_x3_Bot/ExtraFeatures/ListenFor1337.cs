﻿using Discord;
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
            TablesHeader dbTable = new TablesHeader(db);

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

            
            // check if message contains a mention and get this mention as id to string
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

            // Split Message and count every 1337 and @1337 and count them
            string[] userMessage = message.Content.Split(' ');
            int counter1337 = 0;
            int counterM1337 = 0;
            // count
            foreach (var item in userMessage)
            {
                if (item == "1337")
                {
                    counter1337++;
                }
                else if (item == "<@&" + rolles + ">")
                {
                    counterM1337++;
                }
            }
            
            // check if message has 1337 or @1337 ones!
            if (counter1337 == 1  && counterM1337 != 1 || counter1337 != 1 && counterM1337 == 1)
            {
                // select the curent users userid from database
                var table1337 = dbTable.Table1337;
                var userID = from table in table1337
                             where table.userid == message.Author.Id
                             select table.userid;

                // Check if this user is in Database
                if (userID.Count() == 0)
                {
                    // Add new User to Database if not exist
                    Table1337 addNewUser = new Table1337()
                    {
                        userid = message.Author.Id,
                        username = context.Guild.GetUser(message.Author.Id).Username,
                        counter_all = 1,
                        counter_streak = 1,
                        counter_longest_streak = 1,
                        date_begin = DateTime.Today.ToShortDateString(),
                        date_last = DateTime.Today.ToShortDateString()
                    };

                    dbTable.Table1337.InsertOnSubmit(addNewUser);
                    dbTable.SubmitChanges();
                }
                else // Update user if allready exist
                {
                    //get colum from current user by id
                    var userID2 = from table in table1337
                                  where table.userid == message.Author.Id
                                  select table;

                    // temp variable for changes
                    int counter_allQ = 0;
                    int counter_streakQ = 0;
                    int counter_logest_streakQ = 0;
                    string date_lastQ = " ";

                    // get current data from user
                    foreach (var item in userID2)
                    {
                        counter_allQ = item.counter_all;
                        counter_streakQ = item.counter_streak + 1;
                        counter_logest_streakQ = item.counter_longest_streak;
                        date_lastQ = item.date_last;
                    }

                    // update counter longest streak if user got higher streak
                    if (counter_streakQ > counter_logest_streakQ)
                    {
                        counter_logest_streakQ = counter_streakQ;
                    }

                    // get the user which should be updated by id
                    var updateTable = dbTable.Table1337.Single((item) => item.userid == message.Author.Id);

                    // update values for user
                    updateTable.username = context.Guild.GetUser(message.Author.Id).Username;
                    updateTable.counter_all = counter_allQ + 1;
                    updateTable.counter_streak = counter_streakQ; // Muss noch angepasst werden | An Tag anpassen
                    updateTable.counter_longest_streak = counter_logest_streakQ;
                    updateTable.date_last = DateTime.Today.ToShortDateString();

                    // send data to database when SubmitChanges() is called.
                    dbTable.SubmitChanges();
                }
                db.Close();
            
                await context.Channel.SendMessageAsync(context.Guild.GetUser(message.Author.Id).Mention + " Hab dich gezählt :P");
            }
            //await context.Guild.GetTextChannel(325653182736760832).SendMessageAsync(context.Guild.GetUser(message.Author.Id).Nickname + " dein text `" + message + "` wurde gezählt.");
            //await context.Channel.SendMessageAsync(context.Guild.GetUser(message.Author.Id).Nickname + " dein text `" + message + "` wurde gezählt.");

        }
    }
}