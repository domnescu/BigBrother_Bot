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
            DateTime myDateTime = DateTime.Now;
            string Text = message.Text.ToLower();
            string answer;
            //Если сообщение пользователя начинается и содержит "будет" или "следу"
            //к текущей дате добавляется одна неделя.
            if (message.FromId.Value == 135310203 || message.FromId.Value == 241324442)
            {
                User user = new User(message.FromId.Value, client);
                answer = user.FirstName + ", ты говорил что состоишь в секте Нечётной недели поэтому для тебя — Нечётная неделя.";
            }
            else
            {
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
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    answer = "Сейчас " + IsEven + " неделя. С понедельника начнётся " + IsNotEven + " неделя.";
                }
            }
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

