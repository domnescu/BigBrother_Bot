﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    internal class ListOfCommandsTelegram : CommandTelegram
    {
        public override string Name => "Список комманд";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Чтобы мне не приходилось отправлять огромное сообщение, мой создатель написал отдельную статью. \n https://vk.com/@bigbrother_bot-commands",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("команды") && db.CheckText(text, "BotNames")) || message.Text == "/start";
        }
    }
}
