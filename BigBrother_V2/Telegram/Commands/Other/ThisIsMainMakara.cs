using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class ThisIsMainMakaraTelegram : CommandTelegram
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
                    database.AddToDB("INSERT INTO MainMakara (PeerID,Platform) VALUES (" + message.Chat.Id + ",'Telegram');");
                    //database.SetWorkingVariable("MainMakara", message.PeerId.Value.ToString());
                    response = "Сделано! Теперь я буду знать что это общая беседа Макары";
                }
                else
                {
                    response = "Балбес! Ты мне в личку это пишешь ? серьёзно ? Афигеть ты дурень!";
                }
            }
            else if (message.From != null)
            {
                response = "Пересланное сообщение не сработает. А то найдутся всякие дебилы которые будут вводить в меня в заблуждение.";
            }
            else
            {
                response = "Только администратор сообщества имеет доступ к данной команде.";
            }
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            if (text.Contains("запомни") && (text.Contains("главная") || text.Contains("общая")) && text.Contains("беседа") && db.CheckText(text, "BotNames"))
            {
                return true;
            }

            return false;
        }
    }
}
