using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Caffeteria
{
    internal class CaffeteriaSchedule : Command
    {

        public override string Name => "Расписание столовой";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Photo photo_attach = new()
            {
                OwnerId = -187905748,
                AlbumId = 267692087,
                //Id = 457239125
                Id = 457239140
                //Id = 457239112
            };
            @params.PeerId = message.PeerId;
            @params.Message = "Держи, только учти что возможны изменения.";
            @params.Attachments = new[] { photo_attach };
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string Text = message.Text.ToLower();
            return ((Text.Contains("во сколько") || Text.Contains("до скольки") || Text.Contains("когда")) && (Text.Contains("завтрак") || Text.Contains("обед") || Text.Contains("ужин"))) ||
                   (Text.Contains("расписание") && (Text.Contains("столов") || Text.Contains("столов") || Text.Contains("рестора")));
        }
    }
}
