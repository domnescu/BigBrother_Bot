using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet;
using System.Text.RegularExpressions;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class AutoKickShortLinks:Command
    {
        public override string Name => "Автоматический кик за короткие ссылки в беседе Стрельна";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            Database db = new();
            GetConversationMembersResult UsersInChat = client.Messages.GetConversationMembers(message.PeerId.Value);
            for (int i = 0; i < UsersInChat.Count; i++)
            {
                if (message.FromId.Value == UsersInChat.Items[i].MemberId)
                {
                    client.Messages.Delete(conversationMessageIds: new[] { (ulong)message.ConversationMessageId }, (ulong)message.PeerId.Value, deleteForAll: true);

                    if (UsersInChat.Items[i].CanKick == true)
                    {
                        _ = client.Messages.RemoveChatUser((ulong)message.PeerId.Value - 2000000000, message.FromId.Value);

                        @params.Message = db.RandomResponse("ShortLinkInMainMakara");
                        @params.PeerId = message.PeerId.Value;
                        @params.RandomId = new Random().Next();
                        Send(@params, client);
                    }
                    else
                    {
                        @params.Message = "Вот сучоныш! успел съебаться уже!";
                    }
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Regex regex = new(@"(пиcaть\ cюдa\ \-\ vk\.cc/[\s\S]{6,6})");
            MatchCollection matches = regex.Matches(text);
            return matches.Count>=1 && message.PeerId== 2000000014;
        }
    }
}
