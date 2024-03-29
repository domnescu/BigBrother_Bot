﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    internal class WhoIAmTelegram : CommandTelegram
    {
        public override string Name => "Кто Я ?";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database database = new();
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: database.RandomResponse("WhoIAm"),
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("кто такой") && db.CheckText(text, "BotNames");
        }
    }
}
