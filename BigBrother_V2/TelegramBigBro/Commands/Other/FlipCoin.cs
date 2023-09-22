using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    internal class FlipCoinTelegram : CommandTelegram
    {
        public override string Name => "Подбрось монетку";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            string text = new Random().Next() % 2 == 0 ? "На моей цифровой монете, выпала решка." : "Судя по циферкам которые я получил, выпал орёл.";
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.Contains("подбрось") || text.Contains("подкинь")) && text.Contains("монет");
        }
    }
}
