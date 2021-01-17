using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;


namespace BigBrother_V2.Vkontakte.Commands
{
    class KartaPerekrestok : Command
    {
        public override string Name => "Карта Перекрёстка";


        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new MessagesSendParams();
            User user = new User(message.FromId.Value, client);
            if (message.PeerId.Value < 2000000000)
            {
                if (user.Sex == VkNet.Enums.Sex.Male)
                {
                    @params.Message = user.FirstName + ", Ну на тебе карту Перекрёстка";
                }
                else if (user.Sex == VkNet.Enums.Sex.Female)
                {
                    @params.Message = user.FirstName + ", Карта перекрёстка, специально для вас.";
                }
                else
                {
                    @params.Message = "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                }
                Photo photo_attach = new Photo
                {
                    OwnerId = -187905748,
                    AlbumId = 267692087,
                    Id = 457239023
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
            if ((text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && (text.Contains("перекрест") || text.Contains("перекрёст")))
                return true;
            return false;
        }
    }
}
