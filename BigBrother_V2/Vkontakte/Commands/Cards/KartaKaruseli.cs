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

        public override void Execute(Message message, VkApi client)
        {
            User user = new User(message.FromId.Value, client);
            MessagesSendParams @params = new MessagesSendParams();
            if (message.PeerId.Value < 2000000000)
            {
                if (user.Sex == VkNet.Enums.Sex.Male)
                {
                    @params.Message = user.FirstName + ",можешь купить мне пивка ? ";
                }
                else if (user.Sex == VkNet.Enums.Sex.Female)
                {
                    @params.Message = user.FirstName + ",поделись вкусняшками!";
                }
                else
                {
                    @params.Message = "Существо неопозднанного пола, немедленно покинь здание в котором ты находишься! За тобой уже выехали!";
                }
                Photo photo_attach = new Photo
                {
                    OwnerId = -187905748,
                    AlbumId = 267692087,
                    Id = 457239020
                };
                @params.Attachments = new[] { photo_attach };
            }
            else
            {
                @params.Message = user.FirstName + ", карты магазинов доступны только в ЛС.";
            }
            @params.PeerId = message.PeerId;
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
