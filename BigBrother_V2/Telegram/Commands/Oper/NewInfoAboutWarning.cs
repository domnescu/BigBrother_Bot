using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    class NewInfoAboutWarningTelegram : CommandTelegram
    {
        public override string Name => "Новая информация по оперу";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new Database();
            if (message.ForwardFrom == null)
            {
                List<string> Types = db.GetWarningTypes();
                List<string> Locations = db.GetListString("PossibleLocations");
                UserTelegram user = new UserTelegram(message);
                string text = " " + message.Text.ToLower();
                foreach (var type in Types)
                {
                    List<string> Warnings = db.GetWarnings(type);
                    foreach (var warning in Warnings)
                    {
                        if (text.Contains(warning.ToLower()))
                        {
                            for (int i = 0; i < Locations.Count; i++)
                            {
                                string CurrentOper = db.GetWorkingVariable("CurrentOper");
                                if (text.Contains(Locations[i].ToLower()))
                                {
                                    string WarningType = type; //костыль для "красивого" сохранения информации
                                    string TextForSaveInfo;
                                    if (WarningType == "опер")
                                        WarningType = CurrentOper;
                                    string LocationForSave = WarningType + " " + Locations[i];
                                    if (Locations[i] == "к себе")
                                    {
                                        LocationForSave = "По последней информации, " + WarningType + " пошёл к себе";
                                    }
                                    else if (Locations[i] == "ушёл" || Locations[i] == "ушел" || Locations[i] == "ушли"
                                        || Locations[i] == "ушла" || Locations[i] == "вышел" || Locations[i] == "вышли" || Locations[i] == "вышла")
                                    {
                                        LocationForSave = WarningType + " " + Locations[i] + " из ";
                                        for (int j = i + 1; j < Locations.Count; j++)
                                        {
                                            if ((text.Contains(Locations[j]))
                                                && Locations[i] != Locations[j])
                                            {
                                                LocationForSave += Locations[j];
                                                break;
                                            }
                                            if (j == Locations.Count - 1)
                                            {
                                                LocationForSave = WarningType + " гуляет где-то!!!";
                                            }
                                        }
                                    }
                                    TextForSaveInfo = LocationForSave + "\nВремя получения информации " + DateTime.Now.ToString("HH:mm");
                                    db.InfoUpdate(type, TextForSaveInfo);
                                    Message sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: message.Chat.Id,
                                        text: "Вот что я запомнил:\n" + LocationForSave,
                                        cancellationToken: cancellationToken
                                    );
                                    string TextForDistribution = LocationForSave + "\n эту информацию я получил из Телеграма, от @" + message.From.Username;
                                    await MessageDistributionWithTelegram(TextForDistribution);
                                    List<long> MainMakaraChats = db.GetListLong("MainMakara");
                                    foreach (var MainMakara in MainMakaraChats)
                                    {
                                        MessagesSendParams @params = new MessagesSendParams();
                                        @params.PeerId = MainMakara;
                                        @params.RandomId = new Random().Next();
                                        @params.Message = LocationForSave + " - эту инфу я получил из Телеграма от @" + message.From.Username;
                                        try
                                        {
                                            Program.BotClient.Messages.Send(@params);
                                        }
                                        catch
                                        {
                                            db.DeleteChat(MainMakara);
                                        }
                                    }
                                    goto EndForeach;

                                }
                            }
                        }
                    }
                EndForeach: { };
                }
            }
            else
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "К сожалению, мне запретили обрабатывать такую информацию из пересланных сообщений.",
                    cancellationToken: cancellationToken
                );
            }
        }

        public override bool Contatins(Message message)
        {
            string text = " " + message.Text.ToLower();
            Database db = new Database();
            if ((text.Contains("где") == false && text.Contains("?") == false && text.Contains("после") == false && text.Contains("будет") == false &&
                text.Contains("через") == false && text.Contains("пойдёт") == false && text.Contains("что") == false && text.Contains("не ") == false &&
                text.Contains("возможно") == false && text.Contains("сказал") == false && text.Contains("надо") == false && text.Contains("кто-нибудь") == false &&
                text.Contains("кто-то") == false && text.Length < 100 && (db.CheckText(text, "WarningList")) && db.CheckText(text, "PossibleLocations") && Regex.Replace(text, @"[^\d]+", "").Length < 5))
                return true;
            return false;
        }
    }
}
