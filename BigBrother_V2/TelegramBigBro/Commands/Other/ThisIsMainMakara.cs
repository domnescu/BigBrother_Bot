using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    internal class ThisIsMainMakaraTelegram : CommandTelegram
    {

        public override string Name => "Указание на главную беседу Макары.";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new();
            UserTelegram user = new(message);
            string response;
            if (message.From == null && user.IsAdmin)
            {
                if (message.Chat.Type != ChatType.Private)
                {
                    _ = database.AddToDB("INSERT INTO MainMakara (PeerID,Platform) VALUES (" + message.Chat.Id + ",'Telegram');");
                    //database.SetWorkingVariable("MainMakara", message.PeerId.Value.ToString());
                    response = "Сделано! Теперь я буду знать что это общая беседа Макары";
                }
                else
                {
                    response = "Балбес! Ты мне в личку это пишешь ? серьёзно ? Афигеть ты дурень!";
                }
            }
            else
            {
                response = message.From != null
                    ? "Пересланное сообщение не сработает. А то найдутся всякие дебилы которые будут вводить в меня в заблуждение."
                    : "Только администратор сообщества имеет доступ к данной команде.";
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("запомни") && (text.Contains("главная") || text.Contains("общая")) && text.Contains("беседа") && db.CheckText(text, "BotNames");
        }
    }
}
