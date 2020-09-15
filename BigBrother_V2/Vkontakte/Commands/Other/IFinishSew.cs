using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class IFinishSew:Command
    {
        public override string Name => "Пустая Команда";
        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            User user = new User(message.PeerId.Value, client);
            bool Succes = db.AddToDB("DELETE FROM WhoSew WHERE domain='[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]';");

            if (Succes)
                @params.Message = "Хорошо, я запомнил что ты больше не шьёшь.";
            else
                @params.Message = "Так тебя и небыло в списке людей которые умеют шить";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.Contains("могу") && text.Contains("шить") && text.Contains("не")) && message.PeerId.Value < 2000000000)
                return true;
            return false;
        }
    }
}
