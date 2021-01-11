using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;


namespace BigBrother_V2.Vkontakte.Commands
{
    class KartaOKEY : Command
    {
        public override string Name => "Карта ОКЕЙ";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            User user = new User(message.FromId.Value, client);
            if (message.PeerId.Value < 2000000000)
            {
                if (user.Sex == VkNet.Enums.Sex.Male)
                {
                    @params.Message = user.FirstName + ", Окей, держи карту ОКЕЙ";
                }
                else if (user.Sex == VkNet.Enums.Sex.Female)
                {
                    @params.Message = user.FirstName + ", ты далеко забрела! Может тебе стоит вернуться обратно ?";
                }
                else
                {
                    @params.Message = "Я по камерам слежу за тобой, немедленно поставь обратно!";
                }
                Photo photo_attach = new Photo
                {
                    OwnerId = -187905748,
                    AlbumId = 267692087,
                    Id = 457239022
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
            if ((text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && (text.Contains("оке") || text.Contains("okey")))
                return true;
            return false;
        }
    }
}
