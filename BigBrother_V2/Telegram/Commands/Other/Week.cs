using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class WeekTelegram : CommandTelegram
    {
        public override string Name => "Какая неделя ?";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            DateTime myDateTime = DateTime.Now;
            string Text = message.Text.ToLower();
            string answer;
            //Если сообщение пользователя начинается и содержит "будет" или "следу"
            //к текущей дате добавляется одна неделя.

            if ((Text.Contains("будет") || Text.Contains("следу")) && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                myDateTime = myDateTime.AddDays(7);
                answer = "Будет ";
            }
            else
            {
                answer = "Сейчас ";
            }
            //получение номера недели
            int week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(myDateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            string IsEven;
            string IsNotEven;
            if (week % 2 == 1)
            {
                IsNotEven = "нечётная";
                IsEven = "чётная";
            }
            else
            {
                IsNotEven = "чётная";
                IsEven = "нечётная";
            }
            answer += IsEven + " неделя";
            if (DateTime.Now.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                answer = "Сейчас " + IsEven + " неделя. С понедельника начнётся " + IsNotEven + " неделя.";
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: answer,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.StartsWith("какая") && text.Contains("неделя");
        }
    }
}

