using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lilac_x3_Bot.Commands;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Database.Tables;
using Lilac_x3_Bot.Service;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Lilac_x3_Bot.LogBot;

namespace Lilac_x3_Bot.ExtraFeatures
{
    class ListenFor1337
    {
        // Member
        private bool _devMode;
        private int counterUserPerDay = 0;
        private static bool initPostDaylieStatsOnes = false;
        CommandHeader _c = new CommandHeader();
        DiscordSocketClient _client;
        CommandHandlingService _command;
        DatabaseInit dbClass = new DatabaseInit();
        SQLiteConnection db;
        Tools t = new Tools();
        private static string formatDate = "dd.MM.yyyy";
        private static string formatTime = "HH:mm:ss";
        private static string formatFullDate = "dd.MM.yyyy HH:mm:ss";

        public ListenFor1337(DiscordSocketClient client, CommandHandlingService command, bool devMode)
        {
            this._devMode = devMode;
            this._client = client;
            this._command = command;
            this._command.AddAbonents(Message1337);

            if (!initPostDaylieStatsOnes)
            {
                this._client.GuildAvailable += PostDaylieStats;
                CopyTableEveryDay();
                LogMain("PostDaylieStats Inizialisiert", LogLevel.Debug);
                
            }
        }

        private void CopyTableEveryDay()
        {
#pragma warning disable CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
            Task.Run(async () =>
#pragma warning restore CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
            {
                while (true)
                {
                    #region set Trigger Time
                    DateTime aktuelleZeit = DateTime.Now;
                    DateTime triggerZeit = new DateTime(aktuelleZeit.Year, aktuelleZeit.Month, aktuelleZeit.Day, 13, 36, 00, 000);
                    // get the time different to int in millisec
                    int iTriggerZeit = Convert.ToInt32(triggerZeit.Subtract(aktuelleZeit).TotalMilliseconds);
                    if (iTriggerZeit < 0)
                    {
                        triggerZeit = new DateTime(aktuelleZeit.Year, aktuelleZeit.Month, aktuelleZeit.Day, 13, 36, 00, 000).AddDays(1);
                        iTriggerZeit = Convert.ToInt32(triggerZeit.Subtract(aktuelleZeit).TotalMilliseconds);
                    }
                    // sleep until the respons time is 13:38 each day
                    Thread.Sleep(iTriggerZeit);
                    #endregion

                    this.db = this.dbClass.connect();
                    TablesHeader dbTable = new TablesHeader(this.db);

                    #region Delete all Data in Table 1337_2 First
                    var table1337_2result = from table in dbTable.Table1337_2
                                            select table;

                    foreach (var item in table1337_2result)
                    {
                        dbTable.Table1337_2.DeleteOnSubmit(item);
                    }
                    try
                    {
                        dbTable.SubmitChanges();
                        LogMain("Daten in Tabelle 1337_2 erfolgreich gelöscht.", LogLevel.Dev);
                    }
                    catch (Exception e)
                    {
                        LogMain("Beim Löschen der Daten in Tabelle 1337_2 ist ein Fehler aufgetretten.\n" + e, LogLevel.Error);
                    }
                    #endregion

                    #region Copy Data from Table 1337 -> 1337_2
                    var table1337result = from table in dbTable.Table1337
                                          select table;

                    foreach (var item in table1337result)
                    {
                        Table1337_2 newList = new Table1337_2();
                        newList.userid = item.userid;
                        newList.username = item.username;
                        newList.counter_all = item.counter_all;
                        newList.counter_streak = item.counter_streak;
                        newList.counter_longest_streak = item.counter_longest_streak;
                        newList.date_begin = item.date_begin;
                        newList.date_last = item.date_last;
                        dbTable.Table1337_2.InsertOnSubmit(newList);
                    }
                    try
                    {
                        dbTable.SubmitChanges();
                        LogMain("Kopieren der Tabelle 1337 -> 1337_2 war erfolgreich.", LogLevel.Dev);
                    }
                    catch (Exception e)
                    {
                        LogMain("Beim Kopieren der Tabelle 1337 -> 1337_2 ist ein Fehler aufgetretten.\n" + e, LogLevel.Error);
                    }
                    #endregion
                    db.Close();
                }

            });
        }

        private async Task Message1337(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var context = new SocketCommandContext(this._client, message);
            bool check = _c.ReadChannel1337Listen(context);
            if (!check) return;

            // Get Time to Check if we are on right time and prepare the string for it
            string timeNowAsString = DateTime.Now.ToString(formatTime);
            string result = t.RemoveSpecificCharFromString(timeNowAsString, ':');
            int timeNowAsInt = Convert.ToInt32(result);

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

            // Split Message and count every 1337 and @1337
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
            if (counter1337 == 1 && counterM1337 != 1 || counter1337 != 1 && counterM1337 == 1)
            {
                //Check if it is the right time! 133700 - 133800 otherwise break
                if (!(133700 <= timeNowAsInt && 133800 >= timeNowAsInt))
                {
                    if (this._devMode) await _c.SendTo1337ChannelAsync(context.Guild.GetUser(message.Author.Id).Mention +
                        " Nicht in der richtigen Zeit!", context);
                    LogMain("1 " + context.Guild.GetUser(message.Author.Id).Username + " Nicht in der richtigen Zeit!", LogLevel.Log);
                    return;
                }

                this.db = this.dbClass.connect();
                TablesHeader dbTable = new TablesHeader(this.db);

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
                        date_begin = DateTime.Today.ToString(formatDate),
                        date_last = DateTime.Today.ToString(formatDate)
                    };

                    dbTable.Table1337.InsertOnSubmit(addNewUser);
                    dbTable.SubmitChanges();
                    // Counts User for bot respons
                    counterUserPerDay++;
                    if (this._devMode) await _c.SendTo1337ChannelAsync(context.Guild.GetUser(message.Author.Id).Mention +
                        " Hab dich gezählt :P", context);
                    LogMain("2 " + context.Guild.GetUser(message.Author.Id).Username + " Hab dich gezählt :P ", LogLevel.Log);
                }
                else // Update user if allready exist
                {
                    //get colum from current user by id
                    var userID2 = from table in table1337
                                  where table.userid == message.Author.Id
                                  select table;

                    // temp variable for changes
                    uint counter_allQ = 0;
                    uint counter_streakQ = 0;
                    uint counter_logest_streakQ = 0;
                    string date_lastQ = " ";

                    // get current data from user
                    foreach (var item in userID2)
                    {
                        counter_allQ = item.counter_all;
                        counter_streakQ = item.counter_streak + 1;
                        counter_logest_streakQ = item.counter_longest_streak;
                        date_lastQ = item.date_last;
                    }

                    if (date_lastQ != DateTime.Today.ToString(formatDate))
                    {
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
                        if (date_lastQ != DateTime.Today.AddDays(-1).ToString(formatDate))
                        {
                            updateTable.counter_streak = 1;
                        }
                        else
                        {
                            updateTable.counter_streak = counter_streakQ;
                        }
                        // Muss noch angepasst werden | An Tag anpassen
                        updateTable.counter_longest_streak = counter_logest_streakQ;
                        updateTable.date_last = DateTime.Today.ToString(formatDate);

                        // send data to database when SubmitChanges() is called.
                        dbTable.SubmitChanges();
                        // Counts User for bot respons
                        counterUserPerDay++;
                        if (_devMode) await _c.SendTo1337ChannelAsync(context.Guild.GetUser(message.Author.Id).Mention +
                            " Hab dich gezählt :P", context);
                        LogMain("3 " + context.Guild.GetUser(message.Author.Id).Username + " Hab dich gezählt :P ", LogLevel.Log);
                    }
                    else
                    {
                        if (this._devMode) await _c.SendTo1337ChannelAsync(context.Guild.GetUser(message.Author.Id).Mention +
                            " Du wurdest heute schon gezählt. Schummeln gilt nicht!! <:pandabulle:327873024017563649>", context);
                        LogMain("4 " + context.Guild.GetUser(message.Author.Id).Username +
                            " Du wurdest heute schon gezählt. Schummeln gilt nicht!! <:pandabulle:327873024017563649> ", LogLevel.Log);
                    }
                }
                this.db.Close();
            }
        }

#pragma warning disable CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        public async Task PostDaylieStats(SocketGuild guild)
#pragma warning restore CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        {
#pragma warning disable CS4014 // Da auf diesen Aufruf nicht gewartet wird, wird die Ausführung der aktuellen Methode vor Abschluss des Aufrufs fortgesetzt.
            if (initPostDaylieStatsOnes)
            {
                LogMain("PostDaylieStats returned.", LogLevel.Debug);
                return;
            }
            initPostDaylieStatsOnes = true;
            Task.Run(async () =>
            {
                while (true)
                {
                    #region send msg when event is over
                    CommandHeader _c2 = new CommandHeader();
                    SocketTextChannel textChannel;

                    // Get actual time and time for trigger
                    DateTime aktuelleZeit = DateTime.Now;
                    DateTime triggerZeit = new DateTime(aktuelleZeit.Year, aktuelleZeit.Month, aktuelleZeit.Day, 13, 38, 00, 000);
                    // get the time different to int in millisec
                    int iTriggerZeit = Convert.ToInt32(triggerZeit.Subtract(aktuelleZeit).TotalMilliseconds);
                    if (iTriggerZeit < 0)
                    {
                        triggerZeit = new DateTime(aktuelleZeit.Year, aktuelleZeit.Month, aktuelleZeit.Day, 13, 38, 00, 000).AddDays(1);
                        iTriggerZeit = Convert.ToInt32(triggerZeit.Subtract(aktuelleZeit).TotalMilliseconds);
                    }
                    // sleep until the respons time is 13:38 each day
                    Thread.Sleep(iTriggerZeit);

                    // If 1337ChannelID is set
                    if (_client.GetChannel(_c2.GetFeature1337ListenFromChannelID()) != null)
                    {
                        textChannel = (SocketTextChannel)_client.GetChannel(_c2.GetFeature1337ListenFromChannelID());
                        await textChannel.SendMessageAsync("Yaay <a:lilacxYayHyperGif:772468799454052392> Dankeschön fürs mitmachen! <a:lilacxHappyGIF:708754770008997968> Heute wurden ganze " + counterUserPerDay + " Meowies um 1337 gezählt. <a:poggers:684767963156709376>");
                    }
                    // If Channel doesn't exist or 0
                    else if (_c2.GetFeature1337ListenFromChannelID() == 0 || _client.GetChannel(_c2.GetFeature1337ListenFromChannelID()) == null)
                    {
                        await _client.GetGuild(guild.Id).SystemChannel.SendMessageAsync("Yaay <a:lilacxYayHyperGif:772468799454052392> Dankeschön fürs mitmachen! <a:lilacxHappyGIF:708754770008997968> Heute wurden ganze " + counterUserPerDay + " Meowies um 1337 gezählt. <:Pog:655898145963900948>");
                    }
                    counterUserPerDay = 0;
                    #endregion

                    #region Set Counter to 0 for ever User that fail on this day
                    this.db = this.dbClass.connect();
                    TablesHeader dbTable = new TablesHeader(this.db);

                    var table1337 = dbTable.Table1337;

                    var twoDaysBeforDateTime = DateTime.Today.AddDays(-1);

                    foreach (var item in table1337)
                    {
                        string[] lastDate = item.date_last.Split(".");
                        DateTime lastDateTime = new DateTime(Convert.ToInt32(lastDate[2]), Convert.ToInt32(lastDate[1]), Convert.ToInt32(lastDate[0]));
                        if (lastDateTime.Ticks <= twoDaysBeforDateTime.Ticks)
                        {
                            item.counter_streak = 0;
                            LogMain("counter_streak für User " + item.username + " auf 0 gesetzt", LogLevel.Dev);
                        }
                    }
                    try
                    {
                        dbTable.SubmitChanges();
                        LogMain("counter_streak wurden für jeden betroffenen User in Tabelle 1337 erfolgreich auf 0 gesetzt.", LogLevel.Dev);
                    }
                    catch (Exception e)
                    {
                        LogMain("Beim setzen von counter_streak in Tabelle 1337 ist ein Fehler aufgetretten.\n" + e, LogLevel.Error);
                    }
                    db.Close();
                    #endregion
                }
            });
#pragma warning restore CS4014 // Da auf diesen Aufruf nicht gewartet wird, wird die Ausführung der aktuellen Methode vor Abschluss des Aufrufs fortgesetzt.
        }
    }
}
