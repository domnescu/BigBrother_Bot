using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    internal class DekanatNumber : Command
    {
        public override string Name => "Номер Деканата";

        public string Number = "Деканат МЦОО - 8(812)421-35-86";
        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            @params.Message = message.FromId.Value == 143676891 ? "Специально для тебя, существует поиск по сообщениям." : Number;

            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("деканат");
        }
    }
}
