using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class OperNumber : Command
    {
        public override string Name => "Номер оперативного дежурного";

        public string Number = "Номер оперативного дежурного - 8(812)421-33-65";

        MessagesSendParams @params = new();

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("опер"))
            {
                return true;
            }

            return false;
        }
    }
}
