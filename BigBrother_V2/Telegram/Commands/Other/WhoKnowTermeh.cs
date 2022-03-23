using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class WhoKnowTermehTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            List<string> list = db.GetListString("WhoKnowTermeh", condition: "WHERE Platform='Telegram'");
            string response = "Вот этого даже я не знаю.";
            if (list.Count != 0)
            {
                response = "Вот тебе список людей которые, возможно, смогут тебе помочь:\n";
                foreach (string str in list)
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
            if (text.Contains("кто") && (text.Contains("знает") || text.Contains("понимает") || text.Contains("может")) &&
                (text.Contains("теормех") || text.Contains("меканик")))
            {
                return true;
            }

            return false;
        }
    }
}
