using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class GiveMem : Command
    {

        public override string Name => "Дай мем";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            List<long> memes= db.GetListLong("memes");
            Wall wall = new Wall
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
            Database db = new Database();
            if (text.Contains("мем") && text.Contains("дай") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
                return true;
            return false;
        }
    }
}
