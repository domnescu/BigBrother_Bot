﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    internal class DekanatMailTelegram : CommandTelegram
    {
        public override string Name => "Почта Деканата";

        public string Number = "Почта деканата - dekanatoif@mail.ru";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Number,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("почта") || text.Contains("у кого")) && text.Contains("почта") && text.Contains("деканат");
        }
    }
}

