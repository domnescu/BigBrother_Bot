﻿using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class ICanSew : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            User user = new User(message.PeerId.Value, client);
            db.AddToDB("INSERT INTO WhoSew (domain) VALUES ('[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]')");
            @params.Message = "Хорошо, я запомнил что ты можешь шить";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("не") == false && (text.Contains("умею") || text.Contains("могу")) && (text.Contains("шить")) && message.PeerId.Value < 2000000000)
                return true;
            return false;
        }
    }
}
