using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class WhoPrintTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {

            Database db = new Database();
            List<string> list = db.GetListString("WhoPrint", condition: "WHERE Platform='Telegram'");
            string response = "ХЗ! Мне никто не говорил что может распечатать";
            if (list.Count != 0)
            {
                response = "Эти люди могут тебе помчь:\n";
                foreach (var str in list)
                {
                    response += str + Environment.NewLine;
                }
            }
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("печата") && (text.Contains("кто") || text.Contains("кого") || text.Contains("где")))
                return true;
            return false;
        }
    }
}
