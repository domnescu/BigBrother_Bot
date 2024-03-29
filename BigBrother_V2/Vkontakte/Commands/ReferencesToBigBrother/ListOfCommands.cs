﻿using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class ListOfCommands : Command
    {
        public override string Name => "Список комманд";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "Чтобы мне не приходилось отправлять огромное сообщение, мой создатель написал отдельную статью. \n https://vk.com/@bigbrother_bot-commands";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("команды") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames"))) || message.Payload == "{\"command\":\"start\"}" || text == "начать";
        }
    }
}
