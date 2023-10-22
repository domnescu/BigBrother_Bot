using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    internal class Electric_SantehnicNumber : Command
    {
        public override string Name => "Номер вызова сантехника или электрика";

        public string Number = "Вызов электрика, сантехника УГ-4 - 8(812)421-49-02";
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
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("сантехник") || text.Contains("электрик"));
        }
    }
}
