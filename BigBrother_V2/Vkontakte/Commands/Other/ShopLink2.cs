using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class ShopLink2 : Command
    {
        public override string Name => "Ссылка на беседу для продажи.";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "Пожалуйста\nhttps://vk.me/join/PodtGJHTlJ2YO2dgjMALfLsuPP92GcvTrFc=";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("бизнес") && text.Contains("макар");
        }
    }
}
