using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class GiveMem : Command
    {

        public override string Name => "Дай мем";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            List<long> memes = db.GetListLong("memes");
            Wall wall = new()
            {
                FromId = -179011410,
                OwnerId = -179011410,
                Id = memes[new Random().Next(memes.Count)],
                PostType = PostType.Post
            };
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Attachments = new[] { wall };
            Send(@params, client);

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("мем") && (text.Contains("дай") || text.Contains("кинь")) && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames"));
        }
    }
}
