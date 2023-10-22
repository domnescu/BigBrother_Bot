using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class Hello : Command
    {
        public override string Name => "Приветствие";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            string text;
            User user = new(message.FromId.Value, client);
            text = user.IsAdmin
                ? "Здравья желаю товарищ Администратор!"
                : user.Sex == VkNet.Enums.Sex.Female
                    ? "Здравствуйте, мадмуазель " + user.FirstName
                    : user.Sex == VkNet.Enums.Sex.Male ? "Здравствуйте, господин " + user.FirstName : "Ну привет, неопозднанное существо";

            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = text;
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.StartsWith("привет") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames"));
        }
    }
}
