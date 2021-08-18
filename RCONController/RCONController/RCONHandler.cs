﻿using CoreRCON;
using Discord;
using Discord.WebSocket;
using System.Net;
using System.Threading;
using Discord = Discord;
using Log = Log;
using Resource = Resource;

namespace UI
{
    class RCONHandler : DiscordChatUI
    {
        RCON connection;
        public RCONHandler(Discord::ITextChannel inputChannel, Log::Logging logging, string initialmessage, Resource::ResourceSet resources) : base(inputChannel, logging, initialmessage, resources)
        {
            logging.log("[RCON] Connecting to MC...");

            //MCへRCONで接続
            Thread.Sleep(90 * 1000);//MCの起動完了を待つ
            IPAddress serveraddress = IPAddress.Parse("127.0.0.1");
            ushort port = 25575;
            string serverpass = "IMadeThisServer";
            connection = new RCON(serveraddress, port, serverpass);
            connection.OnDisconnected += () =>
            {
                connection.ConnectAsync().GetAwaiter().GetResult();
            };
            connection.ConnectAsync().GetAwaiter().GetResult();
            connection.SendCommandAsync("say RCON起動完了").GetAwaiter().GetResult();
            logging.log("[RCON] Connected.");
        }

        public override void AllMessage(SocketMessage inputMessage)
        {
            if(inputMessage.Channel.Id == this.channel.Id) { return; }
            if (inputMessage.Author.IsBot) { return; }
            if (inputMessage.Content.StartsWith("/")) { connection.SendCommandAsync("say コマンドを実行します。"+inputMessage.Content).GetAwaiter().GetResult();return; }
            connection.SendCommandAsync("say [" + inputMessage.Author.Username + "] " + inputMessage.Content).GetAwaiter().GetResult();
        }

        public override void UIMessageReceived(SocketMessage inputMessage)
        {
            //コマンド実行
            if (inputMessage.Content.StartsWith("/"))
            {
                if (inputMessage.Content.Contains("kill"))
                {
                    WriteToChatLog("そのコマンドは禁止されています。").GetAwaiter().GetResult();
                    return;
                }
                if (inputMessage.Content.Contains("gamemode"))
                {
                    WriteToChatLog("そのコマンドは禁止されています。").GetAwaiter().GetResult();
                    return;
                }
                if (inputMessage.Content.Contains("ban"))
                {
                    WriteToChatLog("そのコマンドは禁止されています。").GetAwaiter().GetResult();
                    return;
                }
                if (inputMessage.Content.Contains("banip"))
                {
                    WriteToChatLog("そのコマンドは禁止されています。").GetAwaiter().GetResult();
                    return;
                }
                connection.SendCommandAsync(inputMessage.Content.Split(char.Parse("/"))[1]).GetAwaiter().GetResult();
                logging.log("[RCON] Command from Discord : " + inputMessage.Content.Split(char.Parse("/"))[1]);
                WriteToChatLog("コマンドを実行しました!").GetAwaiter().GetResult();
                return;
            }
            if (inputMessage.Content == "Stop")
            {
                connection.SendCommandAsync("say [警告]マイクラサーバーがあと１分でシャットダウンします。");
                return;
            }
            //モード変更等//開発中
            //チャット接続
            connection.SendCommandAsync("say [" + inputMessage.Author.Username + "] " + inputMessage.Content).GetAwaiter().GetResult();
        }

        public override void UIReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel inputchannel, SocketReaction inputReaction)
        {
        }
    }
}
