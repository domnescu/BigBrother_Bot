﻿using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class WhoCanSew : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            List<string> list = db.GetListString("WhoSew", condition: "WHERE Platform='VK'");
            @params.Message = "Вот хз! мне ещё не говорили";
            if (list.Count != 0)
            {
                @params.Message = "Кто-то из этих людей точно может шить\n";
                foreach (string str in list)
                {
                    @params.Message += str + Environment.NewLine;
                }
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("кто") && text.Contains(" шить");
        }
    }
}
