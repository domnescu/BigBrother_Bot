using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class Hello : Command
    {
        public override string Name => "Приветствие";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            string text;
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin)
            {
                text = "Здравья желаю товарищ Администратор!";
            }
            else if (user.Sex == VkNet.Enums.Sex.Female)
            {
                text = "Здравствуйте, мадмуазель " + user.FirstName;
            }
            else if (user.Sex == VkNet.Enums.Sex.Male)
            {
                text = "Здравствуйте, господин " + user.FirstName;
            }
            else
            {
                text = "Ну привет, неопозднанное существо";
            }

            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = text;
            Send(@params, client);

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            if (text.StartsWith("привет") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
            {
                return true;
            }

            return false;
        }
    }
}
