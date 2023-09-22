using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    internal class DeleteKeyboardTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            ReplyKeyboardRemove keyboard = new();
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Это сообщение должно убрать кнопки",
                cancellationToken: cancellationToken,
                replyMarkup: keyboard
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("наряд закончился") || text.Contains("убери кнопки") || text.Contains("убрать кнопки");
        }
    }
}
