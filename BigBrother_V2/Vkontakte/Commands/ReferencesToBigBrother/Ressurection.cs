using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class Ressurection : Command
    {
        public override string Name => "С возвращением";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            string text;
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                text = "Да иди ты! Не дал мне нормально отдохнуть!! ";
            }
            else if (user.Sex == VkNet.Enums.Sex.Female)
            {
                text = user.FirstName + ", ты рада что я вернулся ? Спасибо!!";
            }
            else if (user.Sex == VkNet.Enums.Sex.Male)
            {
                text = "Братан, я конечно рад вернуться, но только не ебите мозг)";
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
            if (text.Contains("возвращени") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
            {
                return true;
            }

            return false;
        }
    }
}
