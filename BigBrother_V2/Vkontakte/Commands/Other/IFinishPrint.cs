﻿using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class IFinishPrint : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            User user = new User(message.PeerId.Value, client);
            bool Succes = db.AddToDB("DELETE FROM WhoPrint WHERE domain='[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]';");

            if (Succes)
                @params.Message = "Готово, я тебя удалил из списка людей которые могут распечатать.";
            else
                @params.Message = "Так тебя и нет в списке людей которые могут распечатать";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("печата") && text.Contains("не") && message.PeerId.Value < 2000000000)
                return true;
            return false;
        }
    }
}
