using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class NrOfUsersInDistribution : Command
    {

        public override string Name => "Тестовая команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new Database();
            List<long> conversations = database.GetListLong("Chats");
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            int users = 0;
            int Chats =0;
            foreach(var PeerID in conversations)
            {
                if (PeerID < 2000000000)
                    users++;
                else
                    Chats++;
            }
            @params.Message = "Сейчас на мою рассылку подписаны " + users + " людей и "+ Chats + " бесед";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if (text.Contains("сколько") && text.Contains("людей") && text.Contains("подписа") && db.CheckText(text, "BotNames"))
                return true;
            return false;
        }
    }
}
