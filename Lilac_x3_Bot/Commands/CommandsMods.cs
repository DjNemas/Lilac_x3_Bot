using Discord;
using Discord.Commands;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Database.Tables;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsMods : CommandHeader
    {
        [Command("edituser")]
        public async Task ModulsAsync([Remainder] string args = null)
        {
            // Check if right Channel
            bool check = ReadChannelGeneralAdmin();
            if (!check) return;

            // If Mod Role not Set
            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }

            if (args == null)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    Prefix + "edituser <UserID> <Counter_Streak> <Counter_Highest_Streak> <CounterAll> <Last_Date>` \n" +
                    "**BITTE BEACHTEN**: `<Last_Date>` **MUSS** im Format `DD.MM.YYYY` angegeben werden. Eine falsche Formation **WIRD** zu Fehlern bei den nächsten Zählungen führen!");
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            // Check if to less or much Arguments
            if (countArgs.Length < 5)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu wenige Agumente!");
            }
            else if (countArgs.Length > 5)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente!");
            }
            else
            {
                // Check if Member has ModRole
                bool memberHasModRole = false;
                foreach (var item in Context.Guild.GetRole(ModRoleID).Members)
                {
                    if (item.Id == Context.User.Id)
                    {
                        memberHasModRole = true;
                    }
                }
                // User is Mod or Admin
                if (memberHasModRole || Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator)
                {
                    var db = new DatabaseInit().connect();
                    TablesHeader dbTable = new TablesHeader(db);

                    var table1337 = dbTable.Table1337;
                    Table1337 updateUser;
                    try
                    {
                        // Check if UserID is number
                        ulong userID = 0;
                        try
                        {
                            userID = Convert.ToUInt64(countArgs[0]);
                        }
                        catch (Exception)
                        {
                            await SendToGeneralChannelAdminAsync("<UserID> war keine Zahl!");
                            return;
                        }
                        // Get User from Database
                        updateUser = dbTable.Table1337.Single((item) => item.userid == userID);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("Es wurde kein User mit der ID: " + countArgs[0] + " gefunden");
                        return;
                    }
                    // Check If args are Numbers
                    // <Counter_Streak>
                    try
                    {
                        updateUser.counter_streak = Convert.ToUInt32(countArgs[1]);
                    }
                    catch (Exception e)
                    {
                        await SendToGeneralChannelAdminAsync("Meow:\n" + e);
                        await SendToGeneralChannelAdminAsync("<Counter_Streak> war keine Positive Zahl!");
                        return;
                    }
                    // <Counter_Highest_Streak>
                    try
                    {
                        updateUser.counter_longest_streak = Convert.ToUInt32(countArgs[2]);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("<Counter_Highest_Streak> war keine Positive Zahl!");
                        return;
                    }
                    try
                    {
                        updateUser.counter_all = Convert.ToUInt32(countArgs[3]);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("<CounterAll> war keine Positive Zahl!");
                        return;
                    }

                    updateUser.date_last = countArgs[4];
                    dbTable.SubmitChanges();
                    db.Close();

                    await SendToGeneralChannelAdminAsync("User: " + Context.Guild.GetUser(Convert.ToUInt64(countArgs[0])).Username + " wurde erfolgreich Editiert\n" +
                        "Seine neuen Werte lauten wie Folgt:\n" +
                        ">>> Counter Streak: `" + countArgs[1] + "`\n" +
                        "Counter Longest Streak: `" + countArgs[2] + "`\n" +
                        "Counter Alltime: `" + countArgs[3] + "`\n" +
                        "Date Last: `" + countArgs[4] + "`");
                    t.CWLTextColor("User: " + Context.Guild.GetUser(Convert.ToUInt64(countArgs[0])).Username + " wurde von " + Context.User.Username + " mit der ID " + Context.User.Id + " editiert.", ConsoleColor.Yellow);
                }
                else
                {
                    await SendToGeneralChannelAdminAsync("Du hast keine Berechtigung. Du musst im besitz der `" + Context.Guild.GetRole(ModRoleID).Name + "` Rolle sein.");
                }
            }
        }

        [Command("showuser")]
        public async Task ShowUserAsync([Remainder] string args = null)
        {
            if (!ReadChannelGeneralAdmin()) return;

            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }

            if (args == null)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    Prefix + "showuser <UserID>`");
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length > 1)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente!");
            }
            else
            {
                bool memberHasModRole = false;
                foreach (var item in Context.Guild.GetRole(ModRoleID).Members)
                {
                    if (item.Id == Context.User.Id)
                    {
                        memberHasModRole = true;
                    }
                }

                if (memberHasModRole || Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator)
                {
                    var db = new DatabaseInit().connect();
                    TablesHeader dbTable = new TablesHeader(db);

                    var table1337 = dbTable.Table1337;
                    Table1337 updateUser;
                    try
                    {
                        // Check if UserID is number
                        ulong userID = 0;
                        try
                        {
                            userID = Convert.ToUInt64(countArgs[0]);
                        }
                        catch (Exception)
                        {
                            await SendToGeneralChannelAdminAsync("<UserID> war keine Zahl!");
                            return;
                        }
                        // Get User from Database
                        updateUser = dbTable.Table1337.Single((item) => item.userid == userID);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("Es wurde kein User mit der ID: " + countArgs[0] + " gefunden");
                        return;
                    }

                    await SendToGeneralChannelAdminAsync("User Informationen\n" +
                        ">>> Name: `" + updateUser.username + "`\n" +
                        "ID: `" + updateUser.userid.ToString() + "`\n" +
                        "Counter Streak: `" + updateUser.counter_streak + "`\n" +
                        "Counter Longest Streak: `" + updateUser.counter_longest_streak + "`\n" +
                        "Counter All: `" + updateUser.counter_all + "`\n" +
                        "Date Begin: `" + updateUser.date_begin + "`\n" +
                        "Date Last: `" + updateUser.date_last + "`");
                }
                else
                {
                    await SendToGeneralChannelAdminAsync("Du hast keine Berechtigung. Du musst im besitz der `" + Context.Guild.GetRole(ModRoleID).Name + "` Rolle sein.");
                }

            }
        }

    }
}
