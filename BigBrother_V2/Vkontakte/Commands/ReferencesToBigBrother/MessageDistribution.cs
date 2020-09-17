﻿using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class MessageDistribution : Command
    {
        public override string Name => "Рассылка сообщений";

        MessagesSendParams @params = new MessagesSendParams();

        public override async void Execute(Message message, VkApi client)
        {
            @params = new MessagesSendParams();
            Database db = new Database();
            User user = new User(message.FromId.Value, client);
            Random rnd = new Random();
            List<long> ListOfConversations = db.GetListLong("Chats");
            List<long> Users = new List<long>();
            List<long> Chats = new List<long>();
            int count = 1;
            List<MediaAttachment> mediaAttachments = new List<MediaAttachment>();
            string Text = message.Text.Remove(0, 19);
            string StartOfText = user.Sex switch
            {
                VkNet.Enums.Sex.Male => "[id" + user.Id + " |" + user.FirstName + " " + user.LastName + "] просил передать: \n",
                VkNet.Enums.Sex.Female => "[id" + user.Id + " |" + user.FirstName + " " + user.LastName + "] просила передать: \n",
                VkNet.Enums.Sex.Unknown => "Существо неопознонного пола, именующее себя как [id" + user.Id + " | " + user.FirstName + " " + user.LastName + "], просило передать: \n",
                _ => "Какая-то неведомая хуйня, просила передать: \n",
            };
            @params.DisableMentions = true;
            Text = StartOfText + Text;
            foreach (var a in message.Attachments)
            {
                mediaAttachments.Add(a.Instance);
            }
            @params.Message = Text;
            @params.Attachments = mediaAttachments;
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
                        await SendToUsersIds(@params, client);
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
            await SendToUsersIds(@params, client);
            foreach (var peerID in Chats)
            {
                @params.RandomId = rnd.Next();
                @params.UserIds = null;
                @params.PeerId = peerID;
                Send(@params, client);
            }
            @params.UserIds = null;
            @params.PeerId = message.PeerId.Value;
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
