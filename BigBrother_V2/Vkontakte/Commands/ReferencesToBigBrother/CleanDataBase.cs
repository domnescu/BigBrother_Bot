using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class CleanDataBase : Command
    {
        public override string Name => "Тестовая команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            User user = new User(message.FromId.Value, client);
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            if (user.IsAdmin)
            {
                @params.Message = "Хорошо товарищ Администратор, как закончу, отпишусь.";
                Send(@params, client);
                @params.RandomId = new Random().Next();
                @params.Message = "Из базы данных удалены следующие люди: \n";
                Database db = new Database();
                List<long> Chats = db.GetListLong("Chats");
                var conversations = client.Messages.GetConversations(new GetConversationsParams { Count = 200});
                foreach (var conversationAndLastMessage in conversations.Items)
                {
                    ulong LastMessageId = (ulong)conversationAndLastMessage.Conversation.OutRead;
                    var messages = client.Messages.GetById(new[] { LastMessageId }, new[] { "" });
                    foreach (var LastUnreadMessage in messages)
                    {

                        if ((DateTime.Now - LastUnreadMessage.Date).Value.TotalDays > 14 && LastUnreadMessage.PeerId.Value < 2000000000 && Chats.Contains(LastUnreadMessage.PeerId.Value))
                        {
                            User UserInMessageDistribution = new User(LastUnreadMessage.PeerId.Value, client);
                            @params.Message += "[id" + UserInMessageDistribution.Id + "|" + user.FullName + "]\n";
                            db.DeleteChat(LastUnreadMessage.PeerId.Value);
                        }
                    }
                }
                Send(@params, client);
            } else
            {
                @params.Message = "Товарищ курсант, вы кем себя возомнили ?";
                Send(@params, client);
            }
        }

        public override bool Contatins(Message message)
        {
            Database db = new Database();
            string text = message.Text.ToLower();
            if (text.Contains("почисти") && (text.Contains("бд") || text.Contains("базу")) && db.CheckText(text,"BotNames"))
                return true;
            return false;
        }
    }
}
