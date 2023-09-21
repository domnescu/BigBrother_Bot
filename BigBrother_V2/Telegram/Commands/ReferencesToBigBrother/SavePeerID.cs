using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class SavePeerIDTelegram : CommandTelegram
    {
        public override string Name => "Сохранение идентификатора диалога/беседы";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new();
            bool Succes = database.AddChat(message.Chat.Id, "Telegram");
            string text;
            ReplyKeyboardMarkup keyboard = null;
            if (message.Chat.Id > 0)
            {
                if (Succes)
                {
                    KeyboardButton keyboardButton = new("прекрати пересылать инфу");
                    keyboard = new ReplyKeyboardMarkup(keyboardButton)
                    {
                        ResizeKeyboard = true
                    };
                    text = "Хорошо, я буду присылать тебе всю информацию по оперу";
                }
                else
                {
                    text = "Ты уже есть в моей базе данных. Если тебе не приходит информация, вызови администратора.";
                }
            }
            else
            {
                keyboard = null;
                text = Succes
                    ? "Ваш чат успешно добавлен в базу данных"
                    : "Ваш чат уже есть в моей базе данных, если по каким-то причинам информация сюда не приходит, вызовите администратора.";
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
            return (text.Contains("пересылай") || text.Contains("присылай")) && text.Contains("инфу") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
