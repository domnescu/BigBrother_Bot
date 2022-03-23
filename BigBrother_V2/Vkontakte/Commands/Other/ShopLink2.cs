using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class ShopLink2 : Command
    {
        public override string Name => "Ссылка на беседу для продажи.";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "Пожалуйста\nhttps://vk.me/join/AJQ1d5A_1grjDZ0ArYPhk0rr";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("бизнес") && text.Contains("макар"))
            {
                return true;
            }

            return false;
        }
    }
}
