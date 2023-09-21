using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class WhoPrintTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {

            Database db = new();
            List<string> list = db.GetListString("WhoPrint", condition: "WHERE Platform='Telegram'");
            string response = "ХЗ! Мне никто не говорил что может распечатать";
            if (list.Count != 0)
            {
                response = "Эти люди могут тебе помчь:\n";
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
            return text.Contains("печата") && (text.Contains("кто") || text.Contains("кого") || text.Contains("где"));
        }
    }
}
