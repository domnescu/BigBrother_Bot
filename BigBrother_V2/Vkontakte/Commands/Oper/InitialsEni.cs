using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    class InitialsEni : Command
    {
        public override string Name => "Инициалы Еня";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {

            @params.Message = "Ень С. А.";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("инициалы") && (text.Contains("еня") || text.Contains("ень")))
            {
                return true;
            }

            return false;
        }
    }
}
