using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class SorokinaNumber : Command
    {
        public override string Name => "Номер Сорокиной";

        public string Number = "Заместитель декана по учебной работе Сорокина Елена Юрьевна 8(812)421-35-86";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            @params.Message = Number;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("сороки") || text.Contains("елены юрьевны")))
                return true;
            return false;
        }
    }
}
