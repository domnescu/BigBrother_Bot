using System;
using System.Globalization;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class Week : Command
    {
        public override string Name => "Какая неделя ?";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            DateTime myDateTime;
            string Text = message.Text.ToLower();
            string answer;
            //Если сообщение пользователя начинается и содержит "будет" или "следу"
            //к текущей дате добавляется одна неделя.
            if (Text.Contains("будет") || Text.Contains("следу"))
            {
                myDateTime = DateTime.Now.AddDays(7);
                answer = "Будет ";
            }
            else
            {
                myDateTime = DateTime.Now;
                answer = "Сейчас ";
            }
            //получение номера недели
            int week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(myDateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (week % 2 == 0)
                answer += week.ToString() + " неделя. Значит она чётная";
            else
                answer += week.ToString() + " неделя. Значит она нечетная";
            @params.PeerId = message.PeerId.Value;
            @params.Message = answer;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.StartsWith("какая") && text.Contains("неделя"))
                return true;
            return false;
        }
    }
}

