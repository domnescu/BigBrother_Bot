using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Caffeteria
{
    internal class ReadMenuTelegram : CommandTelegram
    {
        public override string Name => "Чтение меню столовой из БД";


        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string text = message.Text.ToLower();
            DateTime dateTime = DateTime.Now;
            string day;
            string Today;
            string ResponseFromDB = null;
            string Response;
            if (DateTime.Now.Hour > 19 || message.Text.ToLower().Contains("завтра ") || message.Text.ToLower().EndsWith("завтра"))
            {
                day = dateTime.AddDays(1).DayOfWeek.ToString();
                Today = "Завтра ";
            }
            else
            {
                day = dateTime.DayOfWeek.ToString();
                Today = "Сегодня ";
            }
            string answer = null;

            if ((text.StartsWith("что") || text.StartsWith("чё")) && text.Contains("завтрак"))
            {
                ResponseFromDB = db.GetMenu("завтрак", day);
                answer = Today + "на завтрак: " + ResponseFromDB;
            }
            else if ((text.StartsWith("что") || text.StartsWith("чё")) && text.Contains("обед"))
            {
                ResponseFromDB = db.GetMenu("обед", day);
                answer = Today + "на обед: " + ResponseFromDB;
            }
            else if ((text.StartsWith("что") || text.StartsWith("чё")) && text.Contains("ужин"))
            {
                ResponseFromDB = db.GetMenu("ужин", day);
                answer = Today + "на ужин: " + ResponseFromDB;
            }
            else if ((text.StartsWith("что") || text.StartsWith("чем") || text.StartsWith("чё") || text.StartsWith("че")) && (text.Contains("столов") || text.Contains("рестора") || text.Contains("кормят")))
            {
                int hour = DateTime.Now.Hour;
                if (hour - 1 <= 8 && hour + 1 >= 8)
                {
                    ResponseFromDB = db.GetMenu("завтрак", day);
                    answer = Today + "на завтрак: " + ResponseFromDB;
                }
                else if (hour - 2 <= 12 && hour + 3 >= 12)
                {
                    ResponseFromDB = db.GetMenu("обед", day);
                    answer = Today + "на обед: " + ResponseFromDB;
                }
                else if (hour - 3 <= 18 && hour + 1 >= 18)
                {
                    ResponseFromDB = db.GetMenu("ужин", day);
                    answer = Today + "на ужин: " + ResponseFromDB;
                }
            }
            Response = ResponseFromDB == null ? db.RandomResponse("RandomCaffeteria") : answer;
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.Contains("что ") || text.Contains("чем ") || text.Contains("чё ") || text.Contains("че ")) && (text.Contains("столов")
                || text.Contains("рестора") || text.Contains("кормят") || text.Contains("завтрак") || text.Contains("обед") || text.Contains("ужин"));
        }
    }
}
