using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class InitialsIvanov : Command
    {
        public override string Name => "Инициалы Иванова";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {

            @params.Message = "Иванов В.В.";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("инициалы") && text.Contains("иванов");
        }
    }
}
