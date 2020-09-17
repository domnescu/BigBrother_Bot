using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class DekanNumber : Command
    {
        public override string Name => "Номер Кольцова";

        public string Number = "Декан общеинженерного факультета (он же Директор МЦОО) Кольцов Олег Вениаминович 8(812)421-38-97";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("декан ") || text.EndsWith("декан") ||
                text.Contains("директор") || text.EndsWith("декана") || text.Contains("кольцов")))
                return true;
            return false;
        }
    }
}
