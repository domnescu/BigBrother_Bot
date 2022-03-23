using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class Promotion : Command
    {
        public override string Name => "Ответ на комплимент";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = db.RandomResponse("AnswerOnPromotion");
            Send(@params, client);

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            if (db.CheckText(text, "promotions") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
            {
                return true;
            }

            return false;
        }
    }
}
