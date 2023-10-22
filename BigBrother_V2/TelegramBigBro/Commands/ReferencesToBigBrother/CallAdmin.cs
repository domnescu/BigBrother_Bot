using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    internal class CallAdminTelegram : CommandTelegram
    {
        public override string Name => "Вызов администратора";

        public override async Task Execute(Telegram.Bot.Types.Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            MessagesSendParams @params = new();
            UserTelegram user = new(message);
            @params.RandomId = new Random().Next();
            VkNet.Utils.VkCollection<User> admins = Program.BotClientVK.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "187905748", });
            foreach (User admin in admins)
            {
                @params.Message = "Выйди пожалуйста на связь с @" + user.Domain + " в Телеграме. У него возникли какие-то проблемы (возможно он наткнулся на ошибку в моей работе)";
                @params.PeerId = admin.Id;
                @params.RandomId = new Random().Next();
                _ = Program.BotClientVK.Messages.Send(@params);
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Администратор уведомлён. В ближайшее время он обработает ваш запрос.",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Telegram.Bot.Types.Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("зов") || text.Contains("вызывай")) && (text.Contains("админ") || text.Contains("шефа") || text.Contains("начальника") || text.Contains("шефа")) && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}