using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class IPrint : Command
    {
        public override string Name => "Я печатаю";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            _ = db.AddToDB("INSERT INTO WhoPrint (domain,Platform) VALUES ('[id" + user.Id + "|" + user.FullName + "]','VK')");
            @params.Message = "Хорошо, я запомнил что ты можешь распечатать";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("могу") && text.Contains("печата") && text.Contains("не") == false && message.PeerId.Value < 2000000000;
        }
    }
}
