using BigBro.Vk.Donates;
using Quartz;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Donates
{
    /// <summary>
    /// 
    /// </summary>
    class InteractionWithVKDonates : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            VkApi client = new VkApi();
            Database db = new Database();
            bool NewDonate = false;
            client.Authorize(new ApiAuthParams() { AccessToken = db.GetWorkingVariable("BigBroKey") });
            MessagesSendParams @params = new MessagesSendParams();
            var webClient = new WebClient();
            var response = webClient.DownloadString(db.GetWorkingVariable("DonateString"));
            DonatesResponse donatesResponse = JsonSerializer.Deserialize<DonatesResponse>(response);
            @params.RandomId = new Random().Next();
            @params.PeerId = long.Parse(db.GetWorkingVariable("MainMakara"));
            int NrOfActualDonates = 0;
            int sum = 0;
            foreach (var donate in donatesResponse.donates)
            {
                DateTime parsedDate = DateTime.Parse(donate.date);
                if (parsedDate.Month == DateTime.Now.Month && parsedDate.Year == DateTime.Now.Year
                    && parsedDate.Day == DateTime.Now.Day && parsedDate.Hour == DateTime.Now.Hour)
                {
                    NewDonate = true;
                    sum += donate.sum;
                    User user = new User(donate.uid, client);
                    @params.Message += "[id" + user.Id + "|@" + user.Domain + "], ";
                    NrOfActualDonates++;
                }
            }
            int numberOFhours = (int)(sum / 0.32);
            int numberOfdays = numberOFhours / 24;
            numberOFhours %= 24;
            string days;
            string hours;
            if (SecondIsOne(numberOfdays))
                days = "дней";
            else if (numberOfdays % 10 == 0 || numberOfdays % 10 >= 5)
                days = "дней";
            else if (numberOfdays % 10 >= 2 && numberOfdays % 10 <= 4)
                days = "дня";
            else
                days = "день";

            if (SecondIsOne(numberOFhours))
                hours = "часов";
            else if (numberOFhours % 10 == 0 || numberOFhours % 10 >= 5)
                hours = "часов";
            else if (numberOFhours % 10 >= 2 && numberOFhours % 10 <= 4)
                hours = "часа";
            else
                hours = "час";
            string NrOfUsers;
            if (NrOfActualDonates > 1)
                NrOfUsers = "вам";
            else
                NrOfUsers = "тебе";

            @params.Message += "большое спасибо за донат, благодоря " + NrOfUsers + " я могу находится на сервере ещё " + numberOfdays + " " + days + " и " + numberOFhours + " " + hours;
            if (NewDonate)
                client.Messages.Send(@params);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        static bool SecondIsOne(int nr)
        {
            nr %= 100;
            nr /= 10;
            if (nr == 1)
                return true;
            return false;
        }
    }
}
