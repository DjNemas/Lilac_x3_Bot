using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Service
{
    public class InteractionServiceHandler
    {
        private IServiceProvider _service;
        private readonly InteractionService _interactionService;
        private readonly DiscordSocketClient _client;

        public InteractionServiceHandler(InteractionService interactionService, DiscordSocketClient client)
        {
            this._interactionService = interactionService;
            this._client = client;
            this._client.SlashCommandExecuted += SlashCommandExecuted;
        }

        private async Task SlashCommandExecuted(SocketSlashCommand rawMessage)
        {
            var context = new SocketInteractionContext(_client, rawMessage);
            await this._interactionService.ExecuteCommandAsync(context, _service);
        }

        public async Task InitializeAsync(IServiceProvider service)
        {
            this._service = service;
            await this._interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        }

        public async Task RegisterSlashCommandsGuild(ulong guildID)
        {
            await this._interactionService.RegisterCommandsToGuildAsync(guildID);
        }
    }
}
