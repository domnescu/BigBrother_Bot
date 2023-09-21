using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class WhoIAm : Command
    {
        public override string Name => "Кто Я ?";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new();
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = database.RandomResponse("WhoIAm");
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("кто такой") && db.CheckText(text, "BotNames");
        }
    }
}
