using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2
{
    class EventsOn00 : IJob
    {
        /// <summary>
        /// Действия в 00 Минут каждого часа
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            Database database = new();
            if (DateTime.Now.Hour == 00)
            {
                database.CleanTable("Votes");
                database.SetWorkingVariable("WhoIsInCheck", "");
                database.SetWorkingVariable("VoteAcces", "open");
                Dictionary<string, string> warnings = database.GetDictionaryString("WarningList");
                foreach (KeyValuePair<string, string> warning in warnings)
                {
                    if (warning.Value != "опер")
                    {
                        string tempString = database.RandomResponse("RandomNewInfo");
                        tempString = tempString.Replace("{warning}", warning.Value);
                        database.InfoUpdate(warning.Value, tempString);
                    }
                }
            }
            if (DateTime.Now.Hour == 16 && database.GetWorkingVariable("VoteAcces") == "open")
            {
                database.SetWorkingVariable("CurrentOper", "Да хуёзнает кто ");
            }
            if ((DateTime.Now.Day == 31 && DateTime.Now.Month == 12) || (DateTime.Now.Day == 01 && DateTime.Now.Month == 01))
            {
                VkApi client = Program.BotClient;
                Database db = new();
                MessagesSendParams @params = new();
                Audio Track = new()
                {
                    OwnerId = -187905748
                };
                if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 14)
                {
                    @params.Message = "Так)) ещё немного и у кого-то начнётся веселье))\nНастоятельно рекомендую слушать песни которые я буду отправлять))" +
                        "99% что поднимут настроение, если не поднимут, то вы ещё недостаточно выпили!!";
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 15)
                {
                    @params.Message = "С Новым годом! Пусть к чёрту катятся все проблемы! Пусть трезвый Дед Мороз подарит опьяняющее счастье трезвой реальности!" +
                        " Желаю крутых подъёмов в новом году, максимальных доходов, желаемых результатов в делах и невероятно страстных чувств в отношениях!";
                    Track.Id = 456239033;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 16)
                {
                    @params.Message = "Новый год стучится в двери, а это значит, что пора пинком под зад отправить все проблемы, запастись охапками продуктов и" +
                        " с широкою, довольною улыбкой набираться позитива. Улыбайтесь же и веселитесь, гоните в шею все дурные мысли! Как шампанское, бурлите " +
                        "от радости и мечтайте о великих перспективах!";
                    Track.Id = 456239018;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 17)
                {
                    @params.Message = "С Новым годом! Желаю быть чудом весь чудесный год, желаю снегопада денег и вьюги удачи, вихря счастья и бурана любви. " +
                        "Пусть этот год будет суперским и мега-крутым, а в нём пусть будет и драйв, и кайф, и роскошь!";
                    Track.Id = 456239020;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 18)
                {
                    @params.Message = "С Новым годом! Желаю выбросить и носки с дырками, и мысли с глупостями, и сомнения по любому поводу. Надеюсь, твоё " +
                        "поведение было прилежным, и Дед Мороз подарит тебе что-то хорошее. Желаю круто встретить новый год и провести его под громким девизом " +
                        "постоянного счастья!";
                    Track.Id = 456239022;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 19)
                {
                    @params.Message = "Пусть Дед Мороз не забудет исполнить все пожелания, президент в полночь пообещает что-то оригинальное, а соседи наберут " +
                        "самых красочных петард для праздничного салюта. Отдайте уходящему году мешок со всеми долгами, неприятностями и невзгодами, обратный " +
                        "адрес не указывайте. С Новым годом!";
                    Track.Id = 456239037;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 20)
                {
                    @params.Message = "Поздравляю с Новым годом! Пусть всегда удаётся смотреть трезво на жизнь, но в то же время быть в опьянении от счастья. " +
                        "Желаю, чтобы шампусик смыл прошлые обиды и печали, чтобы в новом году было столько удачи и веселья, сколько горошинок в самом большом " +
                        "тазу оливье!";
                    Track.Id = 456239034;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 21)
                {
                    @params.Message = "С Новым годом! Желаю встретить его без похмелья, но с весёлыми приключениями на пятую точку! Пусть всё будет, как в доброй" +
                        " сказке, как в старом хорошем фильме. Желаю, чтобы заливная рыба была совсем не гадостью, чтобы тебя никто не поливал из чайника, чтобы " +
                        "не потянуло ни в баню, ни в Ленинград. Пусть новый год сорит деньгами и бьёт битой всех обидчиков!";
                    Track.Id = 456239035;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 22)
                {
                    @params.Message = "Поздравляю с Новым годом и от души желаю если хреновины, то только к холодцу, если падения, то только в объятия любимого " +
                        "человека, если страха, то только у врагов, если мусора, то только в виде бутылок от дорогого алкоголя и контейнеров от обалденной икры.";
                    Track.Id = 456239036;
                }
                else if (DateTime.Now.Day == 31 && DateTime.Now.Hour == 23)
                {
                    @params.Message = "Пусть на утро января не болит голова от выпитого, чтобы желания сбываться начали уже с этого дня. Желаю под елкой найти мешок" +
                        " с деньгами, чтобы хватало на многие года вперед. С Новым годом!";
                    Track.Id = 456239032;
                }
                else if (DateTime.Now.Day == 01 && DateTime.Now.Hour == 0)
                {
                    @params.Message = "Желаю в Новом году всем крепкой печени и внутреннего навигатора, чтобы точно знать как вы пришли туда, где проснулись.";
                    Track.Id = 456239028;
                }
                else if (DateTime.Now.Day == 01 && DateTime.Now.Hour == 01)
                {
                    @params.Message = "С Новым годом. Желаю «не ударить» в Оливье лицом, желаю шампанское и прочие горючие хорошенько закусывать холодцом, желаю" +
                        " в Новом году быть огурцом, желаю утро нового календаря встретить радостно и без похмелья. А ещё желаю здоровья, ума и денег!";
                    Track.Id = 456239023;
                }

                List<long> Chats = db.GetListLong("MainMakara");
                foreach (long chat in Chats)
                {
                    if (Track.Id != null)
                    {
                        @params.Attachments = new[] { Track };
                    }

                    @params.RandomId = new Random().Next();
                    @params.PeerId = chat;
                    client.Messages.Send(@params);
                }
            }
            database.CleanTable("ComandsFromUser");

            return null;
        }
    }

    /// <summary>
    /// Расписание запуска ф-ции в 00 минут
    /// </summary>
    class EventsOn00Scheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<EventsOn00>().Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger2", "group2")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithCronSchedule("0 0 * * * ?")                  // бесконечное повторение
                .Build();                              // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
