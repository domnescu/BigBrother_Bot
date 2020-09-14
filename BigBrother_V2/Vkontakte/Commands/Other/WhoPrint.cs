using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class WhoPrint:Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {

            Database db = new Database();
            List<string> list = db.GetListString("WhoPrint");
            @params.Message = "ХЗ! Мне никто не говорил что может распечатать";
            if (list.Count != 0)
            {
                @params.Message = "Эти люди могут тебе помчь:\n";
                foreach (var str in list)
                {
                    @params.Message += str + "\n";
                }
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("печата") && (text.Contains("кто") || text.Contains("кого") || text.Contains("где")))
                return true;
            return false;
        }
    }
}
