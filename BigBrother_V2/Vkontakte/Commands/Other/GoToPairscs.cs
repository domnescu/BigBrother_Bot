using System;
using System.Threading;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class GoToPairs : Command
    {
        public override string Name => "Идти на пары ?";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Database db = new Database();
            if (new Random().Next() % 2 == 0)
            {
                @params.Message = "Мой псевдорандомайзер говорит что тебе надо пиздовать на пары)";
                Send(@params, client);
            }
            else
            {
                @params.Message = "Рандом говорит пинать хуи. Щяс я его немного исправлю и будет выдавать правильный результат.";
                Send(@params, client);
                @params.RandomId = new Random().Next();
                Thread.Sleep(new Random().Next(0,100)*100);
                @params.Message = db.RandomResponse("GoToPairs");
                Send(@params, client);

            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("идти") && text.Contains("пар") && text.Contains("на"))
                return true;
            return false;
        }
    }
}
