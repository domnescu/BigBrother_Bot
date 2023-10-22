using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class FlipCoin : Command
    {
        public override string Name => "Подбрось монетку";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = new Random().Next() % 2 == 0 ? "На моей цифровой монете, выпала решка." : "Судя по циферкам которые я получил, выпал орёл.";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.Contains("подбрось") || text.Contains("подкинь")) && text.Contains("монет");
        }
    }
}
