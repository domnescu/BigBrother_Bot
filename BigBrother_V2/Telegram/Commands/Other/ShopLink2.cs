using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class ShopLinkTelegram : CommandTelegram
    {
        public override string Name => "Ссылка на беседу для продажи.";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста\nhttps://vk.me/join/AJQ1d5A_1grjDZ0ArYPhk0rr",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("бизнес") && text.Contains("макар");
        }
    }
}
