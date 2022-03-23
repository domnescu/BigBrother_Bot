using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class CleanDataBase : Command
    {
        public override string Name => "Тестовая команда";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            if (user.IsAdmin && message.Type != null)
            {
                @params.Message = "Хорошо товарищ Администратор, как закончу, отпишусь.";
                Send(@params, client);
                @params.RandomId = new Random().Next();
                @params.Message = "Из базы данных удалены следующие люди: \n";
                Database db = new();
                List<long> Chats = db.GetListLong("Chats");
                GetConversationsResult conversations = client.Messages.GetConversations(new GetConversationsParams { Count = 200 });
                foreach (ConversationAndLastMessage conversationAndLastMessage in conversations.Items)
                {
                    ulong LastMessageId = (ulong)conversationAndLastMessage.Conversation.OutRead;
                    VkNet.Utils.VkCollection<Message> messages = client.Messages.GetById(new[] { LastMessageId }, new[] { "" });
                    foreach (Message LastUnreadMessage in messages)
                    {

                        if ((DateTime.Now - LastUnreadMessage.Date).Value.TotalDays > 14 && LastUnreadMessage.PeerId.Value < 2000000000 && Chats.Contains(LastUnreadMessage.PeerId.Value))
                        {
                            User UserInMessageDistribution = new(LastUnreadMessage.PeerId.Value, client);
                            @params.Message += "[id" + UserInMessageDistribution.Id + "|" + UserInMessageDistribution.FullName + "]\n";
                            MessagesSendParams sendParams = new()
                            {
                                PeerId = UserInMessageDistribution.Id,
                                RandomId = new Random().Next()
                            };
                            if (user.Sex == VkNet.Enums.Sex.Male)
                            {
                                sendParams.Message = UserInMessageDistribution.FirstName + ", ты больше 14 дней не читал мои сообщения. По требованию администратора, я удалил тебя из своей Базы Данных.";
                            }
                            else if (user.Sex == VkNet.Enums.Sex.Female)
                            {
                                sendParams.Message = UserInMessageDistribution.FirstName + ", ты больше 14 дней не читала мои сообщения. По требованию администратора, я удалил тебя из своей Базы Данных.";
                            }
                            else
                            {
                                sendParams.Message = UserInMessageDistribution.FirstName + ", ты больше 14 дней не читалО мои сообщения. По требованию администратора, я удалил тебя из своей Базы Данных.";
                            }

                            List<List<MessageKeyboardButton>> buttons = new List<List<MessageKeyboardButton>>
                            {
                                new List<MessageKeyboardButton>()
                                {
                                    new MessageKeyboardButton
                                    {
                                        Action = new MessageKeyboardButtonAction
                                        {
                                            Type = KeyboardButtonActionType.Text,
                                            Label = "Пересылай инфу"
                                        },
                                        Color = KeyboardButtonColor.Primary
                                    }
                                }
                            };
                            sendParams.Keyboard = new MessageKeyboard
                            {
                                Inline = false,
                                OneTime = false,
                                Buttons = buttons
                            };
                            Send(sendParams, client);
                            db.DeleteChat(LastUnreadMessage.PeerId.Value);
                        }
                    }
                }
                Send(@params, client);
            }
            if (message.Type == null)
            {
                @params.Message = user.FirstName + " ты за кого меня принимаешь? Только непосредственно администраторы могут использовать эту команду.";
                Send(@params, client);
            }
            else
            {
                @params.Message = "Товарищ курсант, вы кем себя возомнили ?";
                Send(@params, client);
            }
        }

        public override bool Contatins(Message message)
        {
            Database db = new();
            string text = message.Text.ToLower();
            if (text.Contains("почисти") && (text.Contains("бд") || text.Contains("базу")) && db.CheckText(text, "BotNames"))
            {
                return true;
            }

            return false;
        }
    }
}
