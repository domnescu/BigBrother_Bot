using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class WhoPaint : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            List<string> list = db.GetListString("WhoPaint", condition: "WHERE Platform='VK'");
            @params.Message = "Я бы помог, но сам не знаю";
            if (list.Count != 0)
            {
                @params.Message = "Вот у этих людей можешь спросить:\n";
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
            return text.Contains("кто") && (text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф")) ||
                text.Contains("начерт"));
        }
    }
}
