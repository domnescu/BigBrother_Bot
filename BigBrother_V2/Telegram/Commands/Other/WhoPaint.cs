using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class WhoPaintTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            List<string> list = db.GetListString("WhoPaint", condition: "WHERE Platform='Telegram'");
            string response = "Я бы помог, но сам не знаю";
            if (list.Count != 0)
            {
                response = "Вот у этих людей можешь спросить:\n";
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
            return text.Contains("кто") && (text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф")) ||
                text.Contains("начерт"));
        }
    }
}
