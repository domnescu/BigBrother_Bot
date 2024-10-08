﻿using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class Fouls : Command
    {
        public override string Name => "Ответ на оскорбление";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new User(message.FromId.Value, client);
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            if(user.IsAdmin)
            {
                @params.Message = db.RandomResponse("AnswerOnAdminFoul");
            } else
            {
                @params.Message = db.RandomResponse("AnswerOnFoul");
            }
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("id") == false && db.CheckText(text, "fouls") && ((message.PeerId.Value < 2000000000 && text.Contains("ты") && db.CheckText(text, "WarningList") == false)
                || db.CheckText(text, "BotNames"));
        }
    }
}
