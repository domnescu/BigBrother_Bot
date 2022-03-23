using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class PasspornNumber : Command
    {
        public override string Name => "Номер паспортного стола";

        public string Number = "Паспортного стол 8(929)111-12-71";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("паспорт"))
            {
                return true;
            }

            return false;
        }
    }
}
