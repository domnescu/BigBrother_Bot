using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using Message = VkNet.Model.Message;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    internal class CleanChat : Command
    {
        public override string Name => "Тестовая команда";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            int usersKicked = 0;
            if(user.IsAdmin && message.Type != null)
            {
                @params.Message = "Начинаю чистку беседы от заблокированных пользователей, по окончанию чистки доложу по результатам.";
                @params.PeerId=message.PeerId.Value;
                Send(@params, client);
                GetConversationMembersResult UsersInChat = client.Messages.GetConversationMembers(message.PeerId.Value);

                foreach (var chatUser in UsersInChat.Profiles)
                {
                    if (chatUser.Deactivated== Deactivated.Banned)
                    {
                        _ = client.Messages.RemoveChatUser((ulong)message.PeerId - 2000000000, chatUser.Id);
                        usersKicked++;
                    }
                }
                @params.Message = "По итогу очистки я кикнул из беседы " + usersKicked + " заблокированных аккаунтов.";
                Send(@params, client);

            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("почисти") && text.Contains("беседу") && db.CheckText(text, "BotNames");
        }
    }
}
