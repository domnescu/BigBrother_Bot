using System;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class ShopLink : Command
    {
        public override string Name => "Ссылка на беседу для продажи.";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "Для таких сообщений, есть отдельаня беседа. \n https://vk.me/join/AJQ1d5A_1grjDZ0ArYPhk0rr";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Regex regex = new Regex(@"([0-9]+.?[ \t\v\r\n\f]?((ру?б?)|(₽+)))+");
            MatchCollection matches = regex.Matches(text);
            if (matches.Count > 1 && text.Contains("раз") == false)
                return true;
            return false;
        }
    }
}
