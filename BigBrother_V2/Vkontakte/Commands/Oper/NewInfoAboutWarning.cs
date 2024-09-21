using BigBrother_V2.Additional;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class NewInfoAboutWarning : Command
    {
        public override string Name => "Новая информация по оперу";

        private readonly MessagesSendParams @params = new();

        public override async void Execute(Message message, VkApi client)
        {
            Database db = new();
            if (message.Type == VkNet.Enums.MessageType.Received)
            {
                List<string> Types = db.GetWarningTypes();
                List<string> Locations = db.GetListString("PossibleLocations");
                User user = new(message.FromId.Value, client);
#if !DEBUG
                var BlackList = client.Groups.GetBanned(187905748);
                foreach (var BannedUser in BlackList)
                {
                    if (BannedUser.Profile.Id == user.Id)
                    {
                        @params.PeerId = message.PeerId.Value;
                        @params.RandomId = new Random().Next();
                        @params.Message = db.RandomResponse("RestrictMessageDistribution");
                        Send(@params, client);
                        return;
                    }
                }
#endif
                string text = " " + message.Text.ToLower();
                foreach (string type in Types)
                {
                    List<string> Warnings = db.GetWarnings(type);
                    foreach (string warning in Warnings)
                    {
                        if (text.Contains(warning.ToLower()) || (message.Payload != null && message.Payload.Contains(warning.ToLower())))
                        {
                            for (int i = 0; i < Locations.Count; i++)
                            {
                                string CurrentOper = db.GetWorkingVariable("CurrentOper");
                                if (text.Contains(Locations[i].ToLower()) || (message.Payload != null && message.Payload.Contains(Locations[i])))
                                {
                                    string WarningType = type; //костыль для "красивого" сохранения информации
                                    string TextForSaveInfo;
                                    if (WarningType == "опер")
                                    {
                                        WarningType = CurrentOper;
                                    }

                                    string LocationForSave = WarningType + " " + Locations[i];
                                    if (Locations[i] == "к себе")
                                    {
                                        LocationForSave = "По последней информации, " + WarningType + " пошёл к себе";
                                    }
                                    else if (Locations[i] is "ушёл" or "ушел" or "ушли"
                                        or "ушла" or "вышел" or "вышли" or "вышла")
                                    {
                                        LocationForSave = WarningType + " " + Locations[i] + " из ";
                                        for (int j = i + 1; j < Locations.Count; j++)
                                        {
                                            if(((text.Contains(Locations[j]) || (message.Payload != null && message.Payload.Contains(Locations[j])))
                                                && Locations[i] != Locations[j])&& db.GetString("PossibleLocations", "Location", Locations[j], 3) == "yes")
                                            {
                                                LocationForSave += Locations[j];
                                                break;
                                            } else if (db.GetString("PossibleLocations", "Location", Locations[j], 3) == "no")
                                            {
                                                @params.PeerId = message.PeerId.Value;
                                                @params.RandomId = new Random().Next();
                                                @params.Message = "Да ну нахуй! Я этот бред обрабатывать не буду.";
                                                Send(@params, client);
                                            }
                                            if (j == Locations.Count - 1)
                                            {
                                                LocationForSave = WarningType + " находится в состоянии суперпозиции!!";
                                            }
                                        }
                                    }
                                    TextForSaveInfo = LocationForSave + "\nВремя получения информации " + DateTime.Now.ToString("HH:mm") + "\nИнформация получена от: "+ user.Domain;
                                    db.InfoUpdate(type, TextForSaveInfo);
                                    db.SetWorkingVariable("PeerForAnihilation", message.PeerId.Value.ToString());
                                    @params.UserIds = null;
                                    @params.PeerId = message.PeerId.Value;
                                    @params.RandomId = new Random().Next();
                                    @params.Message = "Вот что я запомнил:\n" + LocationForSave;
                                    Send(@params, client);
                                    @params.DisableMentions = true;
                                    @params.Message = LocationForSave + "\nэту информацию я получил из ВК от ";
                                    StringForLink @string = new()
                                    {
                                        VK = "[id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]",
                                        Telegram = "<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstNameGen + " " + user.LastNameGen + "</a>"
                                    };
                                    await MessageDistribution(@params, client, @string);
                                    List<long> MainMakaraChats = db.GetListLong("MainMakara");
                                    foreach (long MainMakara in MainMakaraChats)
                                    {
                                        if (message.PeerId.Value != MainMakara)
                                        {
                                            @params.PeerId = MainMakara;
                                            @params.RandomId = new Random().Next();
                                            @params.Message = LocationForSave + "\nэту инфу я получил из ВК от " + @string.VK;
                                            Send(@params, client);
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
                @params.Message = "К сожалению, мне запретили обрабатывать такую информацию из пересланных сообщений.";
                @params.PeerId = message.PeerId.Value;
                @params.RandomId = new Random().Next();
                Send(@params, client);
            }
        }

        public override bool Contatins(Message message)
        {
            string text = " " + message.Text.ToLower();
            Database db = new();
            return (text.Contains("где") == false && text.Contains("?") == false && text.Contains("после") == false && text.Contains("будет") == false &&
                text.Contains("через") == false && text.Contains("пойдёт") == false && text.Contains("что") == false && text.Contains("не ") == false &&
                text.Contains("возможно") == false && text.Contains("сказал") == false && text.Contains("надо") == false && text.Contains("кто-нибудь") == false &&
                text.Contains("кто-то") == false && text.Length < 100 && db.CheckText(text, "WarningList") && db.CheckText(text, "PossibleLocations") && Regex.Replace(text, @"[^\d]+", "").Length < 5) || (message.Payload != null && message.Payload.Contains("location"));
        }
    }
}
