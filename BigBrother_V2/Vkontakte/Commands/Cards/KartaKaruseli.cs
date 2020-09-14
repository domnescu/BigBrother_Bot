using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class KartaKaruseli : Command
    {
        public override string Name => "Карта Карусели";

        MessagesSendParams @params = new MessagesSendParams();
        public override void Execute(Message message, VkApi client)
        {
            string text;
            User user = new User(message.FromId.Value, client);
            if (user.Sex == VkNet.Enums.Sex.Male)
            {
                text = user.FirstName + ",можешь купить мне пивка ? ";
            }
            else if (user.Sex == VkNet.Enums.Sex.Female)
            {
                text = user.FirstName + ",поделись вкусняшками!";
            }
            else
            {
                text = "Существо неопозднанного пола, немедленно покинь здание в котором ты находишься! За тобой уже выехали!";
            }
            Photo photo_attach = new Photo
            {
                OwnerId = -187905748,
                AlbumId = 267692087,
                Id = 457239020
            };
            @params.PeerId = message.PeerId;
            @params.Message = text;
            @params.Attachments = new[] { photo_attach };
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && text.Contains("карусел"))
                return true;
            return false;
        }
    }
}
