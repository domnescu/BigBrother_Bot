using System;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class CallAdmin : Command
    {
        public override string Name => "Вызов администратора";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            User user = new User(message.FromId.Value, client);
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            if (message.FromId.Value > 0)
            {
                @params.Message = "Администратор уведомлён. В ближайшее время он обработает ваш запрос.";
                @params.PeerId = message.PeerId;
                @params.RandomId = new Random().Next();
                Send(@params, client);
                var admins = client.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "187905748", });
                foreach (var admin in admins)
                {
                    if (message.PeerId.Value < 2000000000)
                        @params.Message = user.FirstName + " " + user.LastName + " нуждается в помощи. Посмотри пожалуйста сообщения сообщества и попробуй разобраться.";
                    else
                    {

                        @params.Message = "Выйди пожалуйста на связь с [id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]. Этот человек, в какой-то беседе, просил вызвать администратора.";
                    }
                    @params.PeerId = admin.Id;
                    @params.RandomId = new Random().Next();
                    Send(@params, client);
                }
            }
            else
            {
                @params.Message = "Ты чё псина ? Какой админ ? Пошли выйдем, поговорим по-мужски, а то чы чёт берега попутал!";
                @params.PeerId = message.PeerId;
                @params.RandomId = new Random().Next();
                Send(@params, client);
            }


        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            //Добавить упоминания бота
            if ((text.Contains("зов") || text.Contains("вызывай")) && (text.Contains("админ") || text.Contains("шефа") || text.Contains("начальника") || text.Contains("шефа")) && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
                return true;
            return false;
        }
    }
}