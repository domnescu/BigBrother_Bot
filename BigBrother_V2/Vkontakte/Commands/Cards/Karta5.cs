using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class Karta5 : Command
    {
        public override string Name => "Карта Пятёрочки";


        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new MessagesSendParams();
            User user = new User(message.FromId.Value, client);
            if (user.Sex == VkNet.Enums.Sex.Male)
            {
                @params.Message = user.FirstName + ", Карта Пятёрочки, специально для тебя";
            }
            else if (user.Sex == VkNet.Enums.Sex.Female)
            {
                @params.Message = user.FirstName + ", давай договоримся, я тебе карту Пятёрочки, а ты поделишься со мной вкусняшками. Как тебе предложение ? ";
            }
            else
            {
                @params.Message = "Существо неопознанного пола, немедленно покинь магазин! Мало кому нравятся существа неопознанного пола";
            }
            Photo photo_attach = new Photo
            {
                OwnerId = -187905748,
                AlbumId = 267692087,
                Id = 457239062
            };
            @params.Attachments = new[] { photo_attach };
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && (text.Contains("пятёр") || text.Contains("пятер")))
                return true;
            return false;
        }
    }
}
