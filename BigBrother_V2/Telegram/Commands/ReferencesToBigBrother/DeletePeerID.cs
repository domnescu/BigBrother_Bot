using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class DeletePeerIDTelegram : CommandTelegram
    {
        public override string Name => "Удаление диалога из БД";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new();
            bool Succes = database.DeleteChat(message.Chat.Id);
            string text;
            ReplyKeyboardMarkup keyboard = null;
            if (message.Chat.Id > 0)
            {
                if (Succes)
                {
                    KeyboardButton keyboardButton = new("пересылай инфу");
                    keyboard = new ReplyKeyboardMarkup(keyboardButton)
                    {
                        ResizeKeyboard = true
                    };
                    text = "Хорошо, не буду больше беспокоить тебя.";
                }
                else
                {
                    text = "Видимо где-то произошла ошибка, попробуй вызвать Администратора.";
                }
            }
            else
            {
                keyboard = null;
                text = Succes
                    ? "Хорошо, не буду больше отправлять вам информацию."
                    : "Что-то пошло не по плану. Попробуйте связаться с Админом сообщества.";
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken,
                replyMarkup: keyboard
            );
        }

        public override bool Contatins(Message message)
        {
            Database db = new();
            string text = message.Text.ToLower();
            return (text.Contains("прекрати") || text.Contains("перестань")) && text.Contains("инф") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
