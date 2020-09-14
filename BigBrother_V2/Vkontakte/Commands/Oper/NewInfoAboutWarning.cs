using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class NewInfoAboutWarning : Command
    {
        public override string Name => "Новая информация по оперу";

        MessagesSendParams @params = new MessagesSendParams();

        public override async void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            if (message.Type == VkNet.Enums.MessageType.Received)
            {
                List<string> Types = db.GetWarningTypes();
                List<string> Locations = db.GetListString("PossibleLocations");
                User user = new User(message.FromId.Value, client);
                string text = message.Text.ToLower();
                foreach (var type in Types)
                {
                    List<string> Warnings = db.GetWarnings(type);
                    foreach (var warning in Warnings)
                    {
                        if (text.Contains(warning.ToLower()) || (message.Payload != null && message.Payload.Contains(warning.ToLower())))
                        {
                            for (int i = 0; i < Locations.Count; i++)
                            {
                                string CurrentOper = db.GetWorkingVariable("CurrentOper");
                                if (text.Contains(Locations[i].ToLower()) || text.StartsWith("ень") || (message.Payload != null && message.Payload.Contains(Locations[i])))
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
                                            if ((text.Contains(Locations[j]) || (message.Payload != null && message.Payload.Contains(Locations[j])))
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
                                    @params.DisableMentions = true;
                                    @params.Message = LocationForSave + "\n эту информацию я получил от [id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]";
                                    await MessageDistribution(@params, client);
                                    @params.UserIds = null;
                                    @params.PeerId = message.PeerId.Value;
                                    @params.RandomId = new Random().Next();
                                    @params.Message = "Вот что я запомнил:\n" + LocationForSave;
                                    Send(@params, client);
                                    long MainMakara = long.Parse(db.GetWorkingVariable("MainMakara"));
                                    if (message.PeerId.Value != MainMakara)
                                    {
                                        @params.PeerId = MainMakara;
                                        @params.RandomId = new Random().Next();
                                        @params.Message = LocationForSave + " - эту инфу я получил от [id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]";
                                        Send(@params, client);
                                    }
                                    goto EndFor;

                                }
                            }
                        }
                    }
                EndFor: { };
                }
            }
            else
                @params.Message = "К сожалению, мне запретили обрабатывать такую информацию из пересланных сообщений.";
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if ((text.Contains("где") == false && text.Contains("?") == false && text.Contains("после") == false && text.Contains("будет") == false && text.Contains("пойдёт") == false
                && text.Contains("что") == false && text.Contains("не ") == false && text.Contains("кто-нибудь") == false && text.Contains("кто-то") == false
                && (db.CheckText(text, "WarningList") || text.StartsWith("ень")) && db.CheckText(text, "PossibleLocations") && Regex.Replace(text, @"[^\d]+", "").Length < 5) || (message.Payload != null && message.Payload.Contains("location")))
                return true;
            return false;
        }
    }
}
