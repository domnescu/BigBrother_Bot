using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class HospitalSchedule : Command
    {
        public override string Name => "Расписание 64 Поликлиники";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Photo photo_attach = new Photo
            {
                OwnerId = -187905748,
                AlbumId = 267692087,
                Id = 457239036
            };
            @params.PeerId = message.PeerId;
            @params.Message = "64 Поликлиника";
            @params.Attachments = new[] { photo_attach };
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("расписание") && (text.Contains("больницы") || text.Contains("поликлиники")))
                return true;
            return false;
        }
    }
}
