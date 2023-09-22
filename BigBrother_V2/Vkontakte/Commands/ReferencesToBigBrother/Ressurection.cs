using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class Ressurection : Command
    {
        public override string Name => "С возвращением";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            string text;
            User user = new(message.FromId.Value, client);
            text = user.IsAdmin && message.Type != null
                ? "Да иди ты! Не дал мне нормально отдохнуть!! "
                : user.Sex == VkNet.Enums.Sex.Female
                    ? user.FirstName + ", ты рада что я вернулся ? Спасибо!!"
                    : user.Sex == VkNet.Enums.Sex.Male
                                    ? "Братан, я конечно рад вернуться, но только не ебите мозг)"
                                    : "Ну привет, неопозднанное существо";

            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = text;
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("возвращени") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames"));
        }
    }
}
