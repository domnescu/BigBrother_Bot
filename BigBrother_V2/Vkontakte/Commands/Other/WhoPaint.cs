using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class WhoPaint : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            List<string> list = db.GetListString("WhoPaint", condition: "WHERE Platform='VK'");
            @params.Message = "Я бы помог, но сам не знаю";
            if (list.Count != 0)
            {
                @params.Message = "Вот у этих людей можешь спросить:\n";
                foreach (var str in list)
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
            if (text.Contains("кто") && ((text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф"))) ||
                text.Contains("начерт")))
                return true;
            return false;
        }
    }
}
