using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class Source:Command
    {
        public override string Name => "Отправка ссылки на исходники бота";

        MessagesSendParams @params = new MessagesSendParams();

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
            Database db = new Database();
            if ((text.Contains("исходник") || (text.Contains("код") && text.Contains("исходный"))) && db.CheckText(text,"BotNames"))
                return true;
            return false;
        }
    }
}
