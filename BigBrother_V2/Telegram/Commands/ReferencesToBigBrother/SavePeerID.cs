using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    class SavePeerIDTelegram : CommandTelegram
    {
        public override string Name => "Сохранение идентификатора диалога/беседы";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new Database();
            bool Succes = database.AddChat(message.Chat.Id,"Telegram");
            string text;
            ReplyKeyboardMarkup keyboard = null;
            if (message.Chat.Id > 0)
            {
                if (Succes)
                {
                    KeyboardButton keyboardButton = new KeyboardButton("прекрати пересылать инфу");
                    keyboard = new ReplyKeyboardMarkup(keyboardButton);
                    keyboard.ResizeKeyboard = true;
                    text = "Хорошо, я буду присылать тебе всю информацию по оперу";
                }
                else
                    text = "Ты уже есть в моей базе данных. Если тебе не приходит информация, вызови администратора.";
            }
            else
            {
                keyboard = null;
                if (Succes)
                    text = "Ваш чат успешно добавлен в базу данных";
                else
                    text = "Ваш чат уже есть в моей базе данных, если по каким-то причинам информация сюда не приходит, вызовите администратора.";
            }
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken,
                replyMarkup: keyboard
            );
        }

        public override bool Contatins(Message message)
        {
            Database db = new Database();
            string text = message.Text.ToLower();
            if ((text.Contains("пересылай") || text.Contains("присылай")) && text.Contains("инфу") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames")))
                return true;
            return false;
        }
    }
}
