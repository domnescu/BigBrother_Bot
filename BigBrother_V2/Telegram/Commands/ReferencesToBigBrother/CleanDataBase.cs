﻿//using System.Threading;
//using System.Threading.Tasks;
//using Telegram.Bot;
//using Telegram.Bot.Types;


//namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
//{
//    class CleanDataBaseTelegram : CommandTelegram
//    {
//        public override string Name => "Тестовая команда";

//        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
//        {
//            User user = new User(message.FromId.Value, client);
//            @params.PeerId = message.PeerId.Value;
//            @params.RandomId = new Random().Next();
//            if (user.IsAdmin && message.Type != null)
//            {
//                @params.Message = "Хорошо товарищ Администратор, как закончу, отпишусь.";
//                Send(@params, client);
//                @params.RandomId = new Random().Next();
//                @params.Message = "Из базы данных удалены следующие люди: \n";
//                Database db = new Database();
//                List<long> Chats = db.GetListLong("Chats");
//                var conversations = client.Messages.GetConversations(new GetConversationsParams { Count = 200 });
//                foreach (var conversationAndLastMessage in conversations.Items)
//                {
//                    ulong LastMessageId = (ulong)conversationAndLastMessage.Conversation.OutRead;
//                    var messages = client.Messages.GetById(new[] { LastMessageId }, new[] { "" });
//                    foreach (var LastUnreadMessage in messages)
//                    {

//                        if ((DateTime.Now - LastUnreadMessage.Date).Value.TotalDays > 14 && LastUnreadMessage.PeerId.Value < 2000000000 && Chats.Contains(LastUnreadMessage.PeerId.Value))
//                        {
//                            User UserInMessageDistribution = new User(LastUnreadMessage.PeerId.Value, client);
//                            @params.Message += "[id" + UserInMessageDistribution.Id + "|" + UserInMessageDistribution.FullName + "]\n";
//                            MessagesSendParams sendParams = new MessagesSendParams();
//                            sendParams.PeerId = UserInMessageDistribution.Id;
//                            sendParams.RandomId = new Random().Next();
//                                sendParams.Message = UserInMessageDistribution.FirstName + ", ты больше 14 дней не читал/читала мои сообщения. По требованию администратора, я удалил тебя из своей Базы Данных.";

//                            var buttons = new List<List<MessageKeyboardButton>>
//                            {
//                                new List<MessageKeyboardButton>()
//                                {
//                                    new MessageKeyboardButton
//                                    {
//                                        Action = new MessageKeyboardButtonAction
//                                        {
//                                            Type = KeyboardButtonActionType.Text,
//                                            Label = "Пересылай инфу"
//                                        },
//                                        Color = KeyboardButtonColor.Primary
//                                    }
//                                }
//                            };
//                            sendParams.Keyboard = new MessageKeyboard
//                            {
//                                Inline = false,
//                                OneTime = false,
//                                Buttons = buttons
//                            };
//                            Send(sendParams, client);
//                            db.DeleteChat(LastUnreadMessage.PeerId.Value);
//                        }
//                    }
//                }
//                Send(@params, client);
//            }
//            if (message.Type == null)
//            {
//                @params.Message = user.FirstName + " ты за кого меня принимаешь? Только непосредственно администраторы могут использовать эту команду.";
//                Send(@params, client);
//            }
//            else
//            {
//                @params.Message = "Товарищ курсант, вы кем себя возомнили ?";
//                Send(@params, client);
//            }
//        }

//        public override bool Contatins(Message message)
//        {
//            Database db = new Database();
//            string text = message.Text.ToLower();
//            if (text.Contains("почисти") && (text.Contains("бд") || text.Contains("базу")) && db.CheckText(text, "BotNames"))
//                return true;
//            return false;
//        }
//    }
//}
