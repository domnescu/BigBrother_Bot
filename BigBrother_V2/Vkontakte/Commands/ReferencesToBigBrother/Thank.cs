using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class Thank : Command
    {
        public override string Name => "Ответ на благодарность";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = db.RandomResponse("Thank");
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if ((text.Contains("спасибо") || text.Contains("пасиб") || text.Contains("спс") || text.Contains("благодарю")) && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
                return true;
            return false;
        }
    }
}
