using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class TelegramLink : Command
    {
        public override string Name => "Приветствие";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Link link = new();
            Uri uri = new("https://t.me/BigBrother_Makara_Bot?start");
            link.Uri = uri;
            link.Title = "Мой телеграм";
            link.IsExternal = true;
            link.Description = "МОЯ ТЕЛЕГА";
            link.Caption = "sfdkjgsfdkjvnbb";
            @params.Attachments = new[] { link };
            Send(@params, client);

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("телег") || text.Contains("telegr")) && text.Contains("дай") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames"));
        }
    }
}
