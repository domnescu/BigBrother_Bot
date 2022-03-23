using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class GoToPairsTelegram : CommandTelegram
    {
        public override string Name => "Идти на пары ?";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string text;
            if (message.Text.ToLower().Contains("тобой") || message.Text.ToLower().Contains("тебя"))
            {
                text = "Не, ну в приницпе, можно)) Почему бы и нет ?";
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: text,
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                if (new Random().Next() % 2 == 0)
                {
                    text = "Мой псевдорандомайзер говорит что тебе надо пиздовать на пары)";
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: text,
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    text = "Рандом говорит пинать хуи. Щяс я его немного исправлю и будет выдавать правильный результат.";
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: text,
                        cancellationToken: cancellationToken
                    );
                    Thread.Sleep(new Random().Next(0, 100) * 100);
                    text = db.RandomResponse("GoToPairs");
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: text,
                        cancellationToken: cancellationToken
                    );

                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("идти") && text.Contains("пар") && text.Contains("на"))
            {
                return true;
            }

            return false;
        }
    }
}
