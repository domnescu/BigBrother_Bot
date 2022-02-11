﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    class DeletePeerIDTelegram : CommandTelegram
    {
        public override string Name => "Удаление диалога из БД";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new Database();
            bool Succes = database.DeleteChat(message.Chat.Id);
            string text;
            ReplyKeyboardMarkup keyboard = null;
            if (message.Chat.Id > 0)
            {
                if (Succes)
                {
                    KeyboardButton keyboardButton = new KeyboardButton("пересылай инфу");
                    keyboard = new ReplyKeyboardMarkup(keyboardButton);
                    keyboard.ResizeKeyboard = true;
                    text = "Хорошо, не буду больше беспокоить тебя.";
                }
                else
                    text = "Видимо где-то произошла ошибка, попробуй вызвать Администратора.";
            }
            else
            {
                keyboard = null;
                if (Succes)
                    text = "Хорошо, не буду больше отправлять вам информацию.";
                else
                    text = "Что-то пошло не по плану. Попробуйте связаться с Админом сообщества.";
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
            if ((text.Contains("прекрати") || text.Contains("перестань")) && text.Contains("инф") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames")))
                return true;
            return false;
        }
    }
}
