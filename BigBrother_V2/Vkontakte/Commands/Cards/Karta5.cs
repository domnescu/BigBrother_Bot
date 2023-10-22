using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Cards
{
    internal class Karta5 : Command
    {
        public override string Name => "Карта Пятёрочки";


        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            @params.Message = user.Sex == VkNet.Enums.Sex.Male
                ? user.FirstName + ", Карта Пятёрочки, специально для тебя"
                : user.Sex == VkNet.Enums.Sex.Female
                    ? user.FirstName + ", давай договоримся, я тебе карту Пятёрочки, а ты поделишься со мной вкусняшками. Как тебе предложение ? "
                    : "Существо неопознанного пола, немедленно покинь магазин! Мало кому нравятся существа неопознанного пола";
            Database db = new();
            Photo photo_attach = new()
            {
                OwnerId = -187905748,
                AlbumId = 267692087,
                Id = db.GetLong("Cards", "Name", "Пятёрочка", 1)
            };
            @params.Attachments = new[] { photo_attach };
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && (text.Contains("пятёр") || text.Contains("пятер"));
        }
    }
}
