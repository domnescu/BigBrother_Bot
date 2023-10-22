using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    internal class SorokinaNumber : Command
    {
        public override string Name => "Номер Сорокиной";

        public string Number = "Заместитель декана по учебной работе Сорокина Елена Юрьевна 8(812)421-35-86";
        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            @params.Message = Number;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("сороки") || text.Contains("елены юрьевны"));
        }
    }
}
