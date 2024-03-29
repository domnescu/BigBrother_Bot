﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    internal class IKnowTermehTelegram : CommandTelegram
    {
        public override string Name => "Я делаю Теормех";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            _ = db.AddToDB("INSERT INTO WhoKnowTermeh (domain,Platform) VALUES ('@" + message.From.Username + "','Telegram')");
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Хорошо, я запомнил что ты можешь помочь с теормехом",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("не") == false && (text.Contains("знаю") || text.Contains("понимаю") || text.Contains("делаю") || (text.Contains("могу") && text.Contains("помочь"))) && (text.Contains("теормех")
                || text.Contains("механик"));
        }
    }
}
