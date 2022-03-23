using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class IFinishPrintTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string response;
            bool Succes = db.AddToDB("DELETE FROM WhoPrint WHERE domain='@" + message.From.Username + "';");

            if (Succes)
            {
                response = "Готово, я тебя удалил из списка людей которые могут распечатать.";
            }
            else
            {
                response = "Так тебя и нет в списке людей которые могут распечатать";
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
            if (text.Contains("печата") && text.Contains("не"))
            {
                return true;
            }

            return false;
        }
    }
}
