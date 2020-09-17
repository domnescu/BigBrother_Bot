using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class WhoCanSew : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            List<string> list = db.GetListString("WhoSew");
            @params.Message = "Вот хз! мне ещё не говорили";
            if (list.Count != 0)
            {
                @params.Message = "Кто-то из этих людей точно может шить\n";
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
            if (text.Contains("кто") && text.Contains("шить"))
                return true;
            return false;
        }
    }
}
