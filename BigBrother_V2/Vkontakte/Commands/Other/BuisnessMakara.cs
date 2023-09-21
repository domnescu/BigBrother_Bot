using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class BuisnessMakara : Command
    {

        public override string Name => "Ссылка на беседу Бизнес Макара";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "Пожалуйста, ссылка на беседу Бизнес Макара https://vk.me/join/AJQ1d5A_1grjDZ0ArYPhk0rr";
            @params.DontParseLinks = true;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("бизнес") && text.Contains("макар") && text.Length < 15;
        }
    }
}
