using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class InitialsLodoff : Command
    {
        public override string Name => "Инициалы Лодова";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {

            @params.Message = "Лодов О. В.";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("инициалы") && text.Contains("лодов");
        }
    }
}
