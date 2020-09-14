using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class WhoIAm : Command
    {
        public override string Name => "Кто Я ?";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new Database();
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = database.RandomResponse("WhoIAm");
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if (text.Contains("кто такой") && db.CheckText(text, "BotNames"))
                return true;
            return false;
        }
    }
}
