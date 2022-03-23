using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    class CallAdminTelegram : CommandTelegram
    {
        public override string Name => "Вызов администратора";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            MessagesSendParams @params = new();
            UserTelegram user = new(message);
            @params.RandomId = new Random().Next();
            VkNet.Utils.VkCollection<VkNet.Model.User> admins = Program.BotClient.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "187905748", });
            foreach (VkNet.Model.User admin in admins)
            {
                @params.Message = "Выйди пожалуйста на связь с @" + user.Domain + " в Телеграме. У него возникли какие-то проблемы (возможно он наткнулся на ошибку в моей работе)";
                @params.PeerId = admin.Id;
                @params.RandomId = new Random().Next();
                Program.BotClient.Messages.Send(@params);
            }
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Администратор уведомлён. В ближайшее время он обработает ваш запрос.",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            if ((text.Contains("зов") || text.Contains("вызывай")) && (text.Contains("админ") || text.Contains("шефа") || text.Contains("начальника") || text.Contains("шефа")) && (message.Chat.Id > 0 || db.CheckText(text, "BotNames")))
            {
                return true;
            }

            return false;
        }
    }
}