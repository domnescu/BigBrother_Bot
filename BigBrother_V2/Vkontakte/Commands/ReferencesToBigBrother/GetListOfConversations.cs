using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class GetListOfConversations : Command
    {
        public override string Name => "Получение списка бесед в которых сидит бб";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            List<long> possibleChats = new();
            //Да, это максимально тупой костыль - я знаю об этом. Знаете более элегатное решение - дайте знать, с радостью исправлю это.
            for(long i = 2000000001; i < 2000000501; i++)
                possibleChats.Add(i);
            @params.Message = "На данный момент я нахожусь в следующих беседах: \n";

            Database db = new Database();
            foreach(long peer_id in possibleChats)
            {
                try
                {
                    ConversationResult chats = client.Messages.GetConversationsById(new[] { peer_id });
                    GetConversationMembersResult UsersInChat = client.Messages.GetConversationMembers(peer_id);
                    @params.Message += "\npeer_id=" + peer_id + " " + chats.Items.ElementAt(0).ChatSettings.Title + ":" + UsersInChat.Count + " участников";
                    db.UpdateChatsList(peer_id);

                } catch
                {
                    db.DeleteChat(peer_id);
                    Console.WriteLine(@"Chat {0} not exists", peer_id);
                }
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("список") && text.Contains("чатов") && db.CheckText(text, "BotNames");
        }
    }
}
