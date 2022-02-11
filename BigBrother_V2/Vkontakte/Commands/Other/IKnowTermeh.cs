using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class IKnowTermeh : Command
    {
        public override string Name => "Я делаю Теормех";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            User user = new User(message.PeerId.Value, client);
            db.AddToDB("INSERT INTO WhoKnowTermeh (domain,Platform) VALUES ('[id" + user.Id + "|" + user.FullName + "]','VK')");
            @params.Message = "Хорошо, я запомнил что ты можешь помочь с теормехом";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("не") == false && (text.Contains("знаю") || text.Contains("понимаю") || text.Contains("делаю") || (text.Contains("могу") && text.Contains("помочь"))) && (text.Contains("теормех")
                || text.Contains("механик")) && message.PeerId.Value < 2000000000)
                return true;
            return false;
        }
    }
}
