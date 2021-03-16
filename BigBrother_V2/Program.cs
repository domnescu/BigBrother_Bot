using BigBrother_V2.Vkontakte.Commands;
using BigBrother_V2.Vkontakte.Commands.Caffeteria;
using BigBrother_V2.Vkontakte.Commands.Cards;
using BigBrother_V2.Vkontakte.Commands.Numbers;
using BigBrother_V2.Vkontakte.Commands.Oper;
using BigBrother_V2.Vkontakte.Commands.Other;
using BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother;
using BigBrother_V2.Vkontakte.Donates;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2
{
    class Program
    {
        /// <summary>
        /// идентификатор сообщества Большой Брат
        /// </summary> 
#if DEBUG
        static ulong bigbroID { get; } = 192662250;
#else
        static ulong bigbroID { get; } = 187905748;
#endif
        /// <summary>
        /// Клиент бота
        /// </summary>
        static VkApi BotClient = new VkApi();
        /// <summary>
        /// Список всех доступных команд
        /// </summary>
        static List<Command> ListOfCommands = new List<Command>();

        static void Main(string[] args)
        {
            Initialize();
            StartLongPoll();
        }
        /// <summary>
        /// Метод отслеживающий обновления
        /// </summary>
        static void StartLongPoll()
        {
            var BigBroLongPollServer = BotClient.Groups.GetLongPollServer(bigbroID);
            while (true)
            {
                try
                {
                    var poll = BotClient.Groups.GetBotsLongPollHistory(
                       new BotsLongPollHistoryParams()
                       {
                           Server = BigBroLongPollServer.Server,
                           Ts = BigBroLongPollServer.Ts,
                           Key = BigBroLongPollServer.Key,
                       });
                    if (poll.Updates == null) continue;
                    BigBroLongPollServer.Ts = poll.Ts;
                    foreach (var update in poll.Updates)
                    {
                        if (update.Type == GroupUpdateType.MessageNew)
                        {
                            //убираем упоминания бота из текста.
                            update.MessageNew.Message.Text = update.MessageNew.Message.Text.Replace("[club187905748|*bigbrother_bot] ", "");
                            update.MessageNew.Message.Text = update.MessageNew.Message.Text.Replace("[club187905748|@bigbrother_bot] ", "");
                            ProcessingMessageAsync(update.MessageNew.Message);
                        }
                        else if (update.Type == GroupUpdateType.MessageDeny)
                        {
                            Database db = new Database();
                            db.DeleteChat(update.MessageDeny.UserId.Value);
                        }
                    }

                }
                catch (LongPollException exception)
                {
                    if (exception is LongPollOutdateException outdateException)
                    {
                        BigBroLongPollServer.Ts = outdateException.Ts;
                    }
                    else
                    {
                        BigBroLongPollServer = BotClient.Groups.GetLongPollServer(bigbroID);
                    }
                }
            }
        }
        /// <summary>
        /// Асинхронная отправка сообщения на обработку, с последующим поиском пересланных сообщений
        /// </summary>
        /// <param name="message">Сообщение пользователя</param>
        static async void ProcessingMessageAsync(Message message)
        {
            await Task.Run(() => ProcessingMessage(message));
            if (message.ForwardedMessages.Count != 0)
            {
                for (int i = 0; i < message.ForwardedMessages.Count; i++)
                {
                    message.ForwardedMessages[i].PeerId = message.PeerId;
                    ProcessingMessageAsync(message.ForwardedMessages[i]);
                }
            }
        }
        /// <summary>
        /// Асинхронный перебор команд
        /// </summary>
        /// <param name="message">Сообщение</param>
        static async void ProcessingMessage(Message message)
        {
            foreach (var command in ListOfCommands)
            {
                await CheckCommandsAsync(command, message);
            }
        }
        /// <summary>
        /// Асинхронная проверка сообщения, на соответствие команды
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        static async Task CheckCommandsAsync(Command command, Message message)
        {
            if (command.Contatins(message))
                await Task.Run(() => command.Execute(message, BotClient));
        }
        /// <summary>
        /// В данном методе происходит добавление всех команд в список ListOfCommands и авторизация бота
        /// </summary>
        static void Initialize()
        {
            Database db = new Database();
            BotClient.Authorize(new ApiAuthParams() { AccessToken = db.GetWorkingVariable("BigBroKey") });
            DonateScheduler.Start();
            EventsOn00Scheduler.Start();
            #region Инициализация команд
#if DEBUG
            ListOfCommands.Add(new ClearCommand());
#endif
            ListOfCommands.Add(new KartaLenta());
            ListOfCommands.Add(new Karta5());
            ListOfCommands.Add(new KartaOKEY());
            ListOfCommands.Add(new KartaPrisma());
            ListOfCommands.Add(new KartaKaruseli());
            ListOfCommands.Add(new KartaIKEA());
            ListOfCommands.Add(new KartaPerekrestok());
            ListOfCommands.Add(new KartaMagnit());
            ListOfCommands.Add(new DekanNumber());
            ListOfCommands.Add(new ChesnokNumber());
            ListOfCommands.Add(new DocumentovodNumber());
            ListOfCommands.Add(new Electric_SantehnicNumber());
            ListOfCommands.Add(new GolovanovNomber());
            ListOfCommands.Add(new Kabinet8Number());
            ListOfCommands.Add(new OperNumber());
            ListOfCommands.Add(new SorokinaNumber());
            ListOfCommands.Add(new UchebCentrElizarNumber());
            ListOfCommands.Add(new Voen_CafedraNomber());
            ListOfCommands.Add(new ZamDekNumber());
            ListOfCommands.Add(new Hello());
            ListOfCommands.Add(new Ressurection());
            ListOfCommands.Add(new Fouls());
            ListOfCommands.Add(new Promotion());
            ListOfCommands.Add(new Restart());
            ListOfCommands.Add(new Thank());
            ListOfCommands.Add(new CallAdmin());
            ListOfCommands.Add(new GiveMem());
            ListOfCommands.Add(new FlipCoin());
            ListOfCommands.Add(new GoToPairs());
            ListOfCommands.Add(new Week());
            ListOfCommands.Add(new DatabaseUpdate());
            ListOfCommands.Add(new WhoIAm());
            ListOfCommands.Add(new InitialsBublik());
            ListOfCommands.Add(new InitialsEni());
            ListOfCommands.Add(new InitialsLodoff());
            ListOfCommands.Add(new InitialsSavuk());
            ListOfCommands.Add(new ShopLink());
            ListOfCommands.Add(new MessageDistribution());
            ListOfCommands.Add(new SavePeerID());
            ListOfCommands.Add(new Vote());
            ListOfCommands.Add(new WhereIsOper());
            ListOfCommands.Add(new NewInfoAboutWarning());
            ListOfCommands.Add(new NewInfoAboutOper());
            ListOfCommands.Add(new WhoIsOper());
            ListOfCommands.Add(new StatusVote());
            ListOfCommands.Add(new ChangeOper());
            ListOfCommands.Add(new ChangeVoteStatus());
            ListOfCommands.Add(new ButtonsForUser());
            ListOfCommands.Add(new ButtonsOnDuty());
            ListOfCommands.Add(new DeleteKeyboard());
            ListOfCommands.Add(new WriteMenu());
            ListOfCommands.Add(new ReadMenu());
            ListOfCommands.Add(new CaffeteriaSchedule());
            ListOfCommands.Add(new AllNumbers());
            ListOfCommands.Add(new ListOfCommands());
            ListOfCommands.Add(new DekanatNumber());
            ListOfCommands.Add(new IFinishPaint());
            ListOfCommands.Add(new IFinishPrint());
            ListOfCommands.Add(new IPaint());
            ListOfCommands.Add(new IPrint());
            ListOfCommands.Add(new WhoPaint());
            ListOfCommands.Add(new WhoPrint());
            ListOfCommands.Add(new HospitalSchedule());
            ListOfCommands.Add(new DeletePeerID());
            ListOfCommands.Add(new ThisIsMainMakara());
            //ListOfCommands.Add(new Nuran());
            ListOfCommands.Add(new Introduction());
            ListOfCommands.Add(new NrOfUsersInDistribution());
            ListOfCommands.Add(new Source());
            ListOfCommands.Add(new ICanSew());
            ListOfCommands.Add(new WhoCanSew());
            ListOfCommands.Add(new IFinishSew());
            ListOfCommands.Add(new IKnowMath());
            ListOfCommands.Add(new WhoKnowMath());
            ListOfCommands.Add(new IFinishMath());
            ListOfCommands.Add(new IKnowTermeh());
            ListOfCommands.Add(new WhoKnowTermeh());
            ListOfCommands.Add(new IFinishTermeh());
            ListOfCommands.Add(new CleanDataBase());
            ListOfCommands.Add(new KartaPlovdiv());
            //ListOfCommands.Add(new PushAll());
            //ListOfCommands.Add(new DisableReactionFor_PushAll());
            ListOfCommands.Add(new DekanatMail());
            ListOfCommands.Add(new ChesnokMail());
            ListOfCommands.Add(new BlackTea());
            ListOfCommands.Add(new Chrome());
            #endregion
        }
    }
}