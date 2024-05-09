using VkNet;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    internal class Clean_Group : Command
    {
        public override string Name => "бб почисти беседу - автоматически кикнет всех \"собачек\"";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                @params.Message = "Начинаю очистку группы от заблокированных пользователей";
                @params.PeerId=message.PeerId.Value;
                Send(@params, client);
#if !DEBUG
                var Users = client.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "187905748" });
#else
                VkNet.Utils.VkCollection<VkNet.Model.User> Users = client.Groups.GetMembers(new GroupsGetMembersParams { GroupId = "192662250" });
#endif
                foreach (var groupUser in Users)
                {
                    if (groupUser.Deactivated== Deactivated.Banned)
                    {
                        client.Groups.RemoveUser(187905748, groupUser.Id);
                    }
                }
                @params.Message = "Готово, группа благополучно очищена от заблокированных пользователей";
                Send(@params, client);

            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("почисти") && text.Contains("группу") && db.CheckText(text, "BotNames");
        }
    }
}
