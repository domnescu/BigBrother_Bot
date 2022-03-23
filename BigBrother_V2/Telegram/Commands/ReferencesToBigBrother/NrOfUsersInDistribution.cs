using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    class NrOfUsersInDistributionTelegram : CommandTelegram
    {

        public override string Name => "Тестовая команда";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new();
            List<long> conversations = database.GetListLong("Chats");
            int users = 0;
            int Chats = 0;
            string Response;
            List<long> ChatsForNrOfUsers = new();
            foreach (long PeerID in conversations)
            {
                if (PeerID < 2000000000)
                {
                    users++;
                }
                else
                {
                    Chats++;
                    ChatsForNrOfUsers.Add(PeerID);
                }
            }
            Response = "Сейчас на мою рассылку подписаны " + users + " людей и " + Chats + " бесед.";
            VkNet.Model.ConversationResult chats = Program.BotClient.Messages.GetConversationsById(ChatsForNrOfUsers);

            foreach (VkNet.Model.Conversation chat in chats.Items)
            {
                VkNet.Model.GetConversationMembersResult UsersInChat = Program.BotClient.Messages.GetConversationMembers(chat.Peer.Id);
                long NrOFUsersInChat = UsersInChat.Count;
                Response += "\n" + chat.ChatSettings.Title + ":" + NrOFUsersInChat + " участников в ВК";
            }
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: Response,
               cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            if (text.Contains("сколько") && text.Contains("людей") && text.Contains("подписа") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames")))
            {
                return true;
            }

            return false;
        }
    }
}
