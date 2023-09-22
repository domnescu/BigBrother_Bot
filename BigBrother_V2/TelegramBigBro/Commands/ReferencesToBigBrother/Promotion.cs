using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    internal class PromotionTelegram : CommandTelegram
    {
        public override string Name => "Ответ на комплимент";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: db.RandomResponse("AnswerOnPromotion"),
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return db.CheckText(text, "promotions") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
