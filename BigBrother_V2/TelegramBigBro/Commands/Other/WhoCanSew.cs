﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    internal class WhoCanSewTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            List<string> list = db.GetListString("WhoSew", condition: "WHERE Platform='Telegram'");
            string response = "Вот хз! мне ещё не говорили";
            if (list.Count != 0)
            {
                response = "Кто-то из этих людей точно может шить\n";
                foreach (string str in list)
                {
                    response += str + Environment.NewLine;
                }
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("кто") && text.Contains(" шить");
        }
    }
}
