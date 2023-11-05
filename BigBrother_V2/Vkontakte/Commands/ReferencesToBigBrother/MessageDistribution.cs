using BigBrother_V2.Additional;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using VkNet;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class MessageDistribution : Command
    {
        public override string Name => "Рассылка сообщений";


        public override async void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            Database db = new();
            User user = new(message.FromId.Value, client);
            Random rnd = new();
            VkNet.Utils.VkCollection<GetBannedResult> BlackList = client.Groups.GetBanned(187905748);
            foreach (GetBannedResult BannedUser in BlackList)
            {
                if (BannedUser.Profile.Id == user.Id)
                {
                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = new Random().Next();
                    @params.Message = db.RandomResponse("RestrictMessageDistribution");
                    Send(@params, client);
                    return;
                }
            }
            List<long> ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='Telegram'");

            StringForLink @string = new()
            {
                VK = "[id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]",
                Telegram = "<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstNameGen + " " + user.LastNameGen + "</a>"
            };

            @params.Message += @string.VK;
            ListOfConversations.Clear();
            List<long> Users = new();
            List<long> Chats = new();
            List<MediaAttachment> mediaAttachments = new();
            string Text = message.Text.Remove(0, 19);
            switch (user.Sex)
            {
                case VkNet.Enums.Sex.Male:
                    @string.VK = "[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "] просил передать:\n";
                    @string.Telegram = "<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstName + " " + user.LastName + "</a> просил передать:\n";
                    break;
                case VkNet.Enums.Sex.Female:
                    @string.VK = "[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "] просила передать:\n";
                    @string.Telegram = "<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstName + " " + user.LastName + "</a> просила передать:\n";
                    break;
                case VkNet.Enums.Sex.Unknown:
                    @string.VK = "Существо неопознонного пола, именующее себя как [id" + user.Id + " | " + user.FirstName + " " + user.LastName + "], просило передать:\n";
                    @string.Telegram = "Существо неопознонного пола, именующее себя как<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstName + " " + user.LastName + "</a> просило передать:\n";
                    break;
                default:
                    @string.VK = "\n";
                    @string.Telegram = "Какая-то неведомая хуйня, просила передать называющее себя <a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstName + " " + user.LastName + "</a> просила передать:\n";
                    break;
            };
            ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='Telegram'");
            foreach (long ChatID in ListOfConversations)
            {
                _ = await Program.botClient.SendTextMessageAsync(
                    chatId: ChatID,
                    text: @string.Telegram + Text,
                    parseMode: ParseMode.Html
                );
            }

            //ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='VK'");
            ListOfConversations = db.GetListLong("BigBrotherChats");
            GetConversationsResult conversations = client.Messages.GetConversations(new GetConversationsParams { Count = 200 });
            foreach (ConversationAndLastMessage conversationAndLastMessage in conversations.Items)
            {
                if(conversationAndLastMessage.Conversation.CanWrite.Allowed)
                {
                    ListOfConversations.Add(conversationAndLastMessage.Conversation.Peer.Id);
                }
            }
            @params.DisableMentions = true;
            foreach (Attachment a in message.Attachments)
            {
                mediaAttachments.Add(a.Instance);
            }
            @params.Message = @string.VK + Text;
            @params.Attachments = mediaAttachments;
            int count = 1;
            foreach (long peerID in ListOfConversations)
            {
                if (peerID < 2000000000)
                {
                    Users.Add(peerID);
                    count++;
                    if (count == 100)
                    {
                        @params.UserIds = Users;
                        @params.RandomId = rnd.Next();
                        _ = await client.Messages.SendToUserIdsAsync(@params);
                        count = 1;
                        Users.Clear();
                    }
                }
                else
                {
                    Chats.Add(peerID);
                }
            }
            if(Users.Count > 0)
            {
                @params.UserIds = Users;
                @params.RandomId = rnd.Next();
                _ = await client.Messages.SendToUserIdsAsync(@params);
            }
            foreach (long peerID in Chats)
            {
                @params.RandomId = rnd.Next();
                @params.UserIds = null;
                @params.PeerId = peerID;
                Send(@params, client);
            }
            @params.UserIds = null;
            @params.PeerId = message.PeerId.Value;
            @params.Attachments = null;
            @params.RandomId = rnd.Next();
            @params.Message = "Готово, твоё сообщение получили последние 200 человек которые использовали мои команды в ЛС а также все беседы в которых я присутствую.";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.StartsWith("бб сделай рассылку");
        }
    }
}
