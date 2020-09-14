using Quartz;
using Quartz.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBrother_V2
{
    class EventsOn00 : IJob
    {
        /// <summary>
        /// Данная функция выполняется ежедневно в 00:00
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            Database database = new Database();
            database.CleanTable("Votes");
            database.SetWorkingVariable("VoteAcces", "open");
            Dictionary<string, string> warnings = database.GetDictionaryString("WarningList");
            foreach (var warning in warnings)
            {
                if (warning.Value != "опер")
                {
                    string tempString = database.RandomResponse("RandomNewInfo");
                    tempString = tempString.Replace("{warning}", warning.Value);
                    database.InfoUpdate(warning.Value, tempString);
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Расписание запуска ф-ции в 00:00
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
                .WithCronSchedule("* 0 0 * * ?")                  // бесконечное повторение
                .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
