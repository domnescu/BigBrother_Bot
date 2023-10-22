using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class IKnowTermeh : Command
    {
        public override string Name => "Я делаю Теормех";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            _ = db.AddToDB("INSERT INTO WhoKnowTermeh (domain,Platform) VALUES ('[id" + user.Id + "|" + user.FullName + "]','VK')");
            @params.Message = "Хорошо, я запомнил что ты можешь помочь с теормехом";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("не") == false && (text.Contains("знаю") || text.Contains("понимаю") || text.Contains("делаю") || (text.Contains("могу") && text.Contains("помочь"))) && (text.Contains("теормех")
                || text.Contains("механик")) && message.PeerId.Value < 2000000000;
        }
    }
}
