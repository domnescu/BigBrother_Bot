using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    class DekanatNumber : Command
    {
        public override string Name => "Номер Деканата";

        public string Number = "Деканат МЦОО - 8(812)421-35-86";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            if (message.FromId.Value == 143676891)
            {
                @params.Message = "Специально для тебя, существует поиск по сообщениям.";
            }
            else
            {
                @params.Message = Number;
            }

            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("деканат"))
            {
                return true;
            }

            return false;
        }
    }
}
