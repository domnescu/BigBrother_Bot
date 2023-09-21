using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class IPaint : Command
    {
        public override string Name => "Я делаю начерт";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            _ = db.AddToDB("INSERT INTO WhoPaint (domain,Platform) VALUES ('[id" + user.Id + "|" + user.FullName + "]','VK')");
            @params.Message = "Хорошо, я запомнил что ты делаешь начерт или инжеграф";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("не") == false && text.Contains("делаю") && (text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф")) ||
                text.Contains("начерт")) && message.PeerId.Value < 2000000000;
        }
    }
}
