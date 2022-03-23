using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class IFinishPaintTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string response;
            bool Succes = db.AddToDB("DELETE FROM WhoPaint WHERE domain='@" + message.From.Username + "';");
            if (Succes)
            {
                response = "Готово, я тебя удалил из списка людей которые могут сделать начерт или инжеграф";
            }
            else
            {
                response = "Так тебя и нет в списке людей которые могут сделать начерт или инжеграф";
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
            if (text.Contains("не") && text.Contains("делаю") && ((text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф"))) ||
                text.Contains("начерт")))
            {
                return true;
            }

            return false;
        }
    }
}
