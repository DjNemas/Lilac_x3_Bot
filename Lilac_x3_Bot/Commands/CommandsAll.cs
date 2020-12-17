using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{

    public class CommandsAll : CommandHeader
    {

        [Command("test1337")]
        [RequireUserPermission(Discord.ChannelPermission.ManageChannels)]
        public async Task Test13337Async()
        {
            bool check = ReadChannel1337();
            if (!check) return;

            await SendTo1337ChannelAsync("1337 klappt!");
        }

        [Command("testgeneral")]
        [RequireUserPermission(Discord.ChannelPermission.ManageChannels)]
        public async Task TestgeneralAsync()
        {
            bool check = ReadChannelGeneral();
            if (!check) return;

            await SendToGeneralChannelAsync("General klappt!");
        }
    }
}
