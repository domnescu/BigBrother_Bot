using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class Nuran : Command
    {

        public override string Name => "Реклама Нурана";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "[id475894534|Нуран] тут главный по снюсу, всё что связано со снюсом, это к нему";
            @params.DisableMentions = true;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("снюс");
        }
    }
}
