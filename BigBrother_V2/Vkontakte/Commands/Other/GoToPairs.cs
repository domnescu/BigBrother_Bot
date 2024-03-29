﻿using System;
using System.Threading;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class GoToPairs : Command
    {
        public override string Name => "Идти на пары ?";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Database db = new();
            if (message.Text.ToLower().Contains("тобой") || message.Text.ToLower().Contains("тебя"))
            {
                @params.Message = "Не, ну в приницпе, можно)) Почему бы и нет ?";
                Send(@params, client);
            }
            else
            {
                if (new Random().Next() % 2 == 0)
                {
                    @params.Message = "Мой псевдорандомайзер говорит что тебе надо пиздовать на пары)";
                    Send(@params, client);
                }
                else
                {
                    @params.Message = "Рандом говорит пинать хуи. Щяс я его немного исправлю и будет выдавать правильный результат.";
                    Send(@params, client);
                    @params.RandomId = new Random().Next();
                    Thread.Sleep(new Random().Next(0, 100) * 100);
                    @params.Message = db.RandomResponse("GoToPairs");
                    Send(@params, client);

                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("идти") && text.Contains("пар") && text.Contains("на");
        }
    }
}
