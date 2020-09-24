using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class WhoKnowMath : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            List<string> list = db.GetListString("WhoKnowMath");
            @params.Message = "Математика без хуйни! На ютубе посмотри его ведосики) норм объясняет";
            if (list.Count != 0)
            {
                @params.Message = "Вот тебе список людей которые, возможно, смогут тебе помочь:\n";
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
            if (text.Contains("кто") && (text.Contains("знает") || text.Contains("понимает") || text.Contains("может")) && 
                (text.Contains("матема") || text.Contains("матан") || text.Contains("вышмат")) )
                return true;
            return false;
        }
    }
}
