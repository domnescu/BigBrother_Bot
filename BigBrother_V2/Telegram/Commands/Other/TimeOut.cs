using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class TimeOutTelegram : CommandTelegram
    {
        public override string Name => "Включение Тайм-аута";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            UserTelegram user = new(message);
            _ = new Message();
            string answer;
            if (user.IsAdmin && message.ForwardFrom == null)
            {
                if (Regex.Replace(message.Text, @"[^\d]+", "").Length != 0)
                {
                    int time = int.Parse(Regex.Replace(message.Text, @"[^\d]+", ""));
                    db.SetWorkingVariable("TimeOut", (DateTime.Now.Minute + time).ToString());
                    answer = "Спасибо за то что даёте возможность отдохнуть.";

                    _ = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: answer,
                        cancellationToken: cancellationToken
                    );

                    Thread.Sleep(60 * 1000 * time);
                    db.SetWorkingVariable("TimeOut", "0");
                    return;
                }
                else
                {
                    answer = "А ты не хочешь указать на сколько мне нужно отключиться ?";
                }
            }
            else
            {
                answer = message.ForwardFrom != null
                    ? "Пересланные сообщения администраторов не обрабатываются."
                    : "Данная команда доступна только для администраторов.";
            }
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: answer,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            Database db = new();
            string text = message.Text.ToLower();
            return (text.Contains("пауз") || text.Contains("тайм")) && db.CheckText(text, "BotNames");
        }
    }
}
