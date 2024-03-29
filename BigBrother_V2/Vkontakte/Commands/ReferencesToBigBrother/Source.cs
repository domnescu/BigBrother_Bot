﻿using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class Source : Command
    {
        public override string Name => "Отправка ссылки на исходники бота";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            @params.Message = "Мой исходный код доступен по ссылке: https://github.com/domnescu/BigBrother_Bot";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("исходник") || (text.Contains("код") && text.Contains("исходный"))) && db.CheckText(text, "BotNames");
        }
    }
}
