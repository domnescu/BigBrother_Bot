using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class WhoKnowTermeh : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            List<string> list = db.GetListString("WhoKnowTermeh", condition: "WHERE Platform='VK'");
            @params.Message = "Вот этого даже я не знаю.";
            if (list.Count != 0)
            {
                @params.Message = "Вот тебе список людей которые, возможно, смогут тебе помочь:\n";
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
            return text.Contains("кто") && (text.Contains("знает") || text.Contains("понимает") || text.Contains("может")) &&
                (text.Contains("теормех") || text.Contains("меканик"));
        }
    }
}
