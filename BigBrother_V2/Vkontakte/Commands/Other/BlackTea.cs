using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class BlackTea : Command
    {

        public override string Name => "Вибратор Чёрного Чая";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "[id449214904|Чай] тут Главный следователь - она уже давно ищет вибратор\n Я бы сказал что она хреновый следователь))";
            @params.DisableMentions = true;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("вибратор");
        }
    }
}
