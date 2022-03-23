using BigBrother_V2.Additional;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class MessageDistribution : Command
    {
        public override string Name => "Рассылка сообщений";


        public override async void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new MessagesSendParams();
            Database db = new Database();
            User user = new User(message.FromId.Value, client);
            Random rnd = new Random();
            var BlackList = client.Groups.GetBanned(187905748);
            foreach (var BannedUser in BlackList)
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

            StringForLink @string = new StringForLink();
            @string.VK = "[id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]";
            @string.Telegram = "<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstNameGen + " " + user.LastNameGen + "</a>";

            @params.Message += @string.VK;
            ListOfConversations.Clear();
            List<long> Users = new List<long>();
            List<long> Chats = new List<long>();
            List<MediaAttachment> mediaAttachments = new List<MediaAttachment>();
            string Text = message.Text.Remove(0, 19);
            switch (user.Sex)
            {
                case VkNet.Enums.Sex.Male:
                    @string.VK ="[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "] просил передать:\n";
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
            foreach (var ChatID in ListOfConversations)
            {
                Telegram.Bot.Types.Message sentMessage = await Program.botClient.SendTextMessageAsync(
                    chatId: ChatID,
                    text: @string.Telegram + Text,
                    parseMode: ParseMode.Html
                );
            }

            ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='VK'");
            @params.DisableMentions = true;
            foreach (var a in message.Attachments)
            {
                mediaAttachments.Add(a.Instance);
            }
            @params.Message = Text;
            @params.Attachments = mediaAttachments;
            int count = 1;
            foreach (var peerID in ListOfConversations)
            {
                if (peerID < 2000000000)
                {
                    Users.Add(peerID);
                    count++;
                    if (count == 100)
                    {
                        @params.UserIds = Users;
                        @params.RandomId = rnd.Next();
                        await client.Messages.SendToUserIdsAsync(@params);
                        count = 1;
                        Users.Clear();
                    }
                }
                else
                {
                    Chats.Add(peerID);
                }
            }
            @params.UserIds = Users;
            @params.RandomId = rnd.Next();
            await client.Messages.SendToUserIdsAsync(@params);
            foreach (var peerID in Chats)
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
            @params.Message = "Я отправил, всем кто подписаны на мою инфу.";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.StartsWith("бб сделай рассылку"))
                return true;
            return false;
        }
    }
}
