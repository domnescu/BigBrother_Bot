using BigBrother_V2.TelegramBigBro.Commands;
using BigBrother_V2.TelegramBigBro.Commands.Caffeteria;
using BigBrother_V2.TelegramBigBro.Commands.Cards;
using BigBrother_V2.TelegramBigBro.Commands.Numbers;
using BigBrother_V2.TelegramBigBro.Commands.Oper;
using BigBrother_V2.TelegramBigBro.Commands.Other;
using BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother;
using BigBrother_V2.Vkontakte.Commands;
using BigBrother_V2.Vkontakte.Commands.Caffeteria;
using BigBrother_V2.Vkontakte.Commands.Cards;
using BigBrother_V2.Vkontakte.Commands.Numbers;
using BigBrother_V2.Vkontakte.Commands.Oper;
using BigBrother_V2.Vkontakte.Commands.Other;
using BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother;
using BigBrother_V2.Vkontakte.Donates;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
        public static VkApi BotClient = new();
        /// <summary>
        /// Список всех доступных команд
        /// </summary>
        static List<Command> ListOfCommands = new();


        public static TelegramBotClient botClient;
        static List<CommandTelegram> CommandsTelegram = new();

        static async Task Main()
        {
            Initialize();
            Database db = new();

            botClient = new TelegramBotClient(db.GetWorkingVariable("BigBroKeyTelegram"));
            using CancellationTokenSource cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // all update types(message, join etc.)
            };
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
            Telegram.Bot.Types.User me = await botClient.GetMeAsync();

            Telegram.Bot.Types.Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: 312191379,
#if !DEBUG
                text: "Успешный запуск на сервере"
#else
                text: "Успешный запуск на Компе"
#endif
            );
            LongPollServerResponse BigBroLongPollServer = BotClient.Groups.GetLongPollServer(bigbroID);
            while (true)
            {
                try
                {
                    BotsLongPollHistoryResponse poll = BotClient.Groups.GetBotsLongPollHistory(
                       new BotsLongPollHistoryParams()
                       {
                           Server = BigBroLongPollServer.Server,
                           Ts = BigBroLongPollServer.Ts,
                           Key = BigBroLongPollServer.Key,
                       });
                    if (poll.Updates == null)
                    {
                        continue;
                    }

                    BigBroLongPollServer.Ts = poll.Ts;
                    foreach (VkNet.Model.GroupUpdate.GroupUpdate update in poll.Updates)
                    {
                        if (update.Type == GroupUpdateType.MessageNew)
                        {
                            //убираем упоминания бота из текста.
                            update.MessageNew.Message.Text = update.MessageNew.Message.Text.Replace("[club187905748|*bigbrother_bot] ", "");
                            update.MessageNew.Message.Text = update.MessageNew.Message.Text.Replace("[club187905748|@bigbrother_bot] ", "");

                            int timer = int.Parse(db.GetWorkingVariable("TimeOut"));
                            if (DateTime.Now.Minute >= timer)
                            {
                                ProcessingMessageAsync(update.MessageNew.Message);
                            }
                        }
                        else if (update.Type == GroupUpdateType.MessageDeny)
                        {
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
            cts.Cancel();
        }
        /// <summary>
        /// Асинхронная отправка сообщения на обработку, с последующим поиском пересланных сообщений
        /// </summary>
        /// <param name="message">Сообщение пользователя</param>
        static async void ProcessingMessageAsync(VkNet.Model.Message message)
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
        static async void ProcessingMessage(VkNet.Model.Message message)
        {
            foreach (Command command in ListOfCommands)
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
        static Task CheckCommandsAsync(Command command, VkNet.Model.Message message)
        {
            if (command.Contatins(message))
            {
                Database db = new();
#if !DEBUG
                db.UserUsedCommandIncrease(message.FromId.Value);
#endif
                if (db.NrOfCommandsFromUser(message.FromId.Value) < 10)
                {
                    try
                    {
                        command.Execute(message, BotClient);
                    }
                    catch (Exception e)
                    {
                        BotClient.Messages.Send(new MessagesSendParams
                        {
                            PeerId = 235052667,
                            RandomId = new Random().Next(),
                            Message = "Произошла ошибка при обработке команд из ВК!!\nОписание ошибки:" + e.Message + "\n\n StackTrace:\n" + e.StackTrace + "\nСообщение которое вызвало ошибку:\n" +
                                message.Text + "\nСообщение пришло от [id" + message.FromId.Value + "|Этого человека]"
                        }); ;
                    }
                }
                else if (db.NrOfCommandsFromUser(message.FromId.Value) == 10)
                {
                    Vkontakte.User user = new(message.FromId.Value, BotClient);
                    BotClient.Messages.Send(new MessagesSendParams
                    {
                        RandomId = new Random().Next(),
                        PeerId = message.PeerId,
                        Message = "[id" + user.Id + "|" + user.Domain + "] превышен лимит команд за час. Все твои команды будут игнорироваться в течение часа.",
                    });
                }

            }
            return Task.CompletedTask;
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClientTelegram, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.MyChatMember && update.MyChatMember.OldChatMember.User.Username == "@BigBrother_Makara_Bot")
            {
                Database db = new();
                db.DeleteChat(update.MyChatMember.Chat.Id);
            }
            // Is update type message?
            if (update.Type != UpdateType.Message)
            {
                return;
            }
            // Is message type text?
            if (update.Message!.Type != MessageType.Text)
            {
                return;
            }

            foreach (CommandTelegram command in CommandsTelegram)
            {
                if (command.Contatins(update.Message))
                {
                    await Task.Run(() => command.Execute(update.Message, botClientTelegram, cancellationToken), cancellationToken);
                }
            }
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }


        /// <summary>
        /// В данном методе происходит добавление всех команд в список ListOfCommands и авторизация бота
        /// </summary>
        static void Initialize()
        {
            Database db = new();
            BotClient.Authorize(new ApiAuthParams() { AccessToken = db.GetWorkingVariable("BigBroKey") });
            DonateScheduler.Start();
            EventsOn00Scheduler.Start();
            #region Инициализация команд
            #region VK Commands
#if DEBUG
            ListOfCommands.Add(new ClearCommand());
#endif
            ListOfCommands.Add(new Karta5());
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
            //ListOfCommands.Add(new PushAll());
            //ListOfCommands.Add(new DisableReactionFor_PushAll());
            ListOfCommands.Add(new DekanatMail());
            ListOfCommands.Add(new ChesnokMail());
            ListOfCommands.Add(new BlackTea());
            //ListOfCommands.Add(new Chrome());
            ListOfCommands.Add(new MedChasti());
            ListOfCommands.Add(new TimeOut());
            ListOfCommands.Add(new Cards());
            ListOfCommands.Add(new PasspornNumber());
            ListOfCommands.Add(new TelegramLink());
            ListOfCommands.Add(new BuisnessMakara());
            ListOfCommands.Add(new NewOper());
            ListOfCommands.Add(new AccountingNumber());
            ListOfCommands.Add(new ShopLink2());
            ListOfCommands.Add(new InitialsIvanov());
            ListOfCommands.Add(new InitialsStepanov());
            ListOfCommands.Add(new Anihilation_Protocol());
            ListOfCommands.Add(new InviteLink());
            ListOfCommands.Add(new Check());
            ListOfCommands.Add(new WhoIsInCheck());
            #endregion
            #region Telegram Commands
#if DEBUG
            CommandsTelegram.Add(new ClearCommandTelegram());
#endif
            CommandsTelegram.Add(new InitialsBublikTelegram());
            CommandsTelegram.Add(new InitialsEniTelegram());
            CommandsTelegram.Add(new InitialsLodoffTelegram());
            CommandsTelegram.Add(new InitialsSavukTelegram());
            CommandsTelegram.Add(new NewInfoAboutOperTelegram());
            CommandsTelegram.Add(new NewInfoAboutWarningTelegram());
            CommandsTelegram.Add(new StatusVoteTelegram());
            CommandsTelegram.Add(new VoteTelegram());
            CommandsTelegram.Add(new WhereIsOperTelegram());
            CommandsTelegram.Add(new WhoIsOperTelegram());
            CommandsTelegram.Add(new CaffeteriaScheduleTelegram());
            CommandsTelegram.Add(new ReadMenuTelegram());
            CommandsTelegram.Add(new WriteMenuTelegram());
            CommandsTelegram.Add(new CardsTelegram());
            CommandsTelegram.Add(new Karta5Telegram());
            CommandsTelegram.Add(new AllNumbersTelegram());
            CommandsTelegram.Add(new ChesnokNumberTelegram());
            CommandsTelegram.Add(new ChesnokMailTelegram());
            CommandsTelegram.Add(new DekanNumberTelegram());
            CommandsTelegram.Add(new DekanatNumberTelegram());
            CommandsTelegram.Add(new DekanatMailTelegram());
            CommandsTelegram.Add(new DocumentovodNumberTelegram());
            CommandsTelegram.Add(new Electric_SantehnicNumberTelegram());
            CommandsTelegram.Add(new GolovanovNomberTelegram());
            CommandsTelegram.Add(new Kabinet8NumberTelegram());
            CommandsTelegram.Add(new MedChastiTelegram());
            CommandsTelegram.Add(new OperNumberTelegram());
            CommandsTelegram.Add(new PasspornNumberTelegram());
            CommandsTelegram.Add(new SorokinaNumberTelegram());
            CommandsTelegram.Add(new UchebCentrElizarNumberTelegram());
            CommandsTelegram.Add(new Voen_CafedraNomberTelegram());
            CommandsTelegram.Add(new ZamDekNumberTelegram());
            CommandsTelegram.Add(new DatabaseUpdateTelegram());
            CommandsTelegram.Add(new FlipCoinTelegram());
            CommandsTelegram.Add(new GoToPairsTelegram());
            CommandsTelegram.Add(new HospitalScheduleTelegram());
            CommandsTelegram.Add(new ICanSewTelegram());
            CommandsTelegram.Add(new IFinishMathTelegram());
            CommandsTelegram.Add(new IFinishPaintTelegram());
            CommandsTelegram.Add(new IFinishPrintTelegram());
            CommandsTelegram.Add(new IFinishSewTelegram());
            CommandsTelegram.Add(new IFinishTermehTelegram());
            CommandsTelegram.Add(new IKnowMathTelegram());
            CommandsTelegram.Add(new IKnowTermehTelegram());
            CommandsTelegram.Add(new IPaintTelegram());
            CommandsTelegram.Add(new IPrintTelegram());
            CommandsTelegram.Add(new ThisIsMainMakaraTelegram());
            CommandsTelegram.Add(new TimeOutTelegram());
            CommandsTelegram.Add(new WeekTelegram());
            CommandsTelegram.Add(new WhoCanSewTelegram());
            CommandsTelegram.Add(new WhoKnowMathTelegram());
            CommandsTelegram.Add(new WhoKnowTermehTelegram());
            CommandsTelegram.Add(new WhoPaintTelegram());
            CommandsTelegram.Add(new WhoPrintTelegram());
            CommandsTelegram.Add(new CallAdminTelegram());
            CommandsTelegram.Add(new ChangeOperTelegram());
            CommandsTelegram.Add(new ChangeVoteStatusTelegram());
            //CommandsTelegram.Add(new CleanDataBaseTelegram());
            CommandsTelegram.Add(new FoulsTelegram());
            CommandsTelegram.Add(new HelloTelegram());
            CommandsTelegram.Add(new IntroductionTelegram());
            CommandsTelegram.Add(new ListOfCommandsTelegram());
            CommandsTelegram.Add(new MessageDistributionTelegram());
            CommandsTelegram.Add(new NrOfUsersInDistributionTelegram());
            CommandsTelegram.Add(new PromotionTelegram());
            CommandsTelegram.Add(new RessurectionTelegram());
            CommandsTelegram.Add(new RestartTelegram());
            CommandsTelegram.Add(new SourceTelegram());
            CommandsTelegram.Add(new ThankTelegram());
            CommandsTelegram.Add(new WhoIAmTelegram());
            CommandsTelegram.Add(new DeletePeerIDTelegram());
            CommandsTelegram.Add(new SavePeerIDTelegram());
            CommandsTelegram.Add(new DeleteKeyboardTelegram());
            CommandsTelegram.Add(new AccountingTelegram());
            CommandsTelegram.Add(new ShopLinkTelegram());
            CommandsTelegram.Add(new InitialsStepanovTelegram());
            CommandsTelegram.Add(new InitialsIvanovTelegram());
            #endregion
            #endregion
        }
    }
}