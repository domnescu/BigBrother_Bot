using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class HospitalSchedule : Command
    {
        public override string Name => "Расписание 64 Поликлиники";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Photo photo_attach = new()
            {
                OwnerId = -187905748,
                AlbumId = 267692087,
                Id = 457239070
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
            return text.Contains("расписание") && (text.Contains("больницы") || text.Contains("поликлиники"));
        }
    }
}
