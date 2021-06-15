﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord = Discord;
using Log = Log;
using Resource = Resource;

namespace UI
{
    class TestUI : DiscordChatUI
    {
        public TestUI(Discord::ITextChannel inputChannel, Log::Logging logging, string initialmessage, Resource::ResourceSet resources) : base(inputChannel, logging, initialmessage, resources)
        {

        }
        public override async Task MessageReceived(Discord::WebSocket.SocketMessage inputMessage)
        {
            if (!inputMessage.Author.IsBot)
            {
                Display = inputMessage.Content;
                logging.log("Mirror working.");
                await refresh();
            }
        }

        public override async Task ReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel inputchannel, SocketReaction inputReaction)
        {
            Display = "\""+inputReaction.Emote.Name+ "\"";
            logging.log(inputReaction.Emote.Name);
            await refresh();
        }
    }
}