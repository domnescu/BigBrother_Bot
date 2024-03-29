﻿using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class Introduction : Command
    {
        public override string Name => "Знакомство с бб";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "Товарищи первокурсники, я местный бот.\nОзнакомиться с списком моих команд, можно в данной статье vk.com/@bigbrother_bot-commands";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("знакомст") || text.Contains("знакомь")) && db.CheckText(text, "BotNames");
        }
    }
}
