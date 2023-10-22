using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class WhoPrint : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {

            Database db = new();
            List<string> list = db.GetListString("WhoPrint", condition: "WHERE Platform='VK'");
            @params.Message = "ХЗ! Мне никто не говорил что может распечатать";
            if (list.Count != 0)
            {
                @params.Message = "Эти люди могут тебе помчь:\n";
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
            return text.Contains("печата") && (text.Contains("кто") || text.Contains("кого") || text.Contains("где"));
        }
    }
}
