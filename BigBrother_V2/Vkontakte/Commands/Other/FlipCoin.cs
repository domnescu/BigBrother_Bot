using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class FlipCoin : Command
    {
        public override string Name => "Подбрось монетку";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            if (new Random().Next() % 2 == 0)
            {
                @params.Message = "На моей цифровой монете, выпала решка.";
            }
            else
            {
                @params.Message = "Судя по циферкам которые я получил, выпал орёл.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.Contains("подбрось") || text.Contains("подкинь")) && text.Contains("монет"))
                return true;
            return false;
        }
    }
}
