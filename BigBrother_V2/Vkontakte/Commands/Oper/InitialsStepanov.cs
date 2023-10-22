using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class InitialsStepanov : Command
    {
        public override string Name => "Инициалы Степанова";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {

            @params.Message = "Степанов Е.И.";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("инициалы") && text.Contains("степанов");
        }
    }
}
