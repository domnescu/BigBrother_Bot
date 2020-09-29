using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    class NewInfoAboutOper : Command
    {
        public override string Name => "Новая информация по оперу";

        MessagesSendParams @params = new MessagesSendParams();

        public override async void Execute(Message message, VkApi client)
        {
            @params = new MessagesSendParams();
            Database db = new Database();
            User user = new User(message.FromId.Value, client);
            List<string> PossibleLocations = db.GetListString("PossibleLocations");
            string text = message.Text.ToLower();
            string warningType;
            string operinfoupdate;
            if (text.StartsWith("вышел") || text.StartsWith("ушёл") || text.StartsWith("ушел") || text.StartsWith("Вернулся"))
            {
                warningType = db.GetWorkingVariable("CurrentOper");
            }
            else
                warningType = "проверка";
            for (int i = 0; i < PossibleLocations.Count; i++)
            {
                if (text.Contains(PossibleLocations[i]))
                {
                    operinfoupdate = warningType + " " + PossibleLocations[i];
                    if (PossibleLocations[i] == "к себе")
                    {
                        operinfoupdate = "По последней информации, " + warningType + " пошёл к себе";
                    }
                    else if (PossibleLocations[i] == "ушёл" || PossibleLocations[i] == "ушел" || PossibleLocations[i] == "ушли" ||
                        PossibleLocations[i] == "ушла" || PossibleLocations[i] == "вышел" || PossibleLocations[i] == "вышли" || PossibleLocations[i] == "вышла")
                    {
                        operinfoupdate = warningType + " " + PossibleLocations[i] + " из ";
                        for (int k = i + 1; k < PossibleLocations.Count; k++)
                        {
                            if (text.Contains(PossibleLocations[k])
                                && PossibleLocations[k] != PossibleLocations[i])
                            {
                                operinfoupdate += PossibleLocations[k];
                                break;
                            }
                            if (k == PossibleLocations.Count - 1)
                            {
                                operinfoupdate = warningType + " гуляет где-то!!!";
                            }
                        }
                    }
                    db.InfoUpdate(warningType, operinfoupdate + "\nВремя получения информации " + DateTime.Now.ToShortTimeString());
                    Random random = new Random();
                    @params.DisableMentions = true;
                    @params.Message = operinfoupdate + " - эту инфу я получил от [id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]";
                    await MessageDistribution(@params, client);
                    @params.UserIds = null;
                    @params.Message = "Вот что я запомнил:\n" + operinfoupdate;
                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = random.Next();
                    Send(@params, client);
                    long MainMakara = long.Parse(db.GetWorkingVariable("MainMakara"));
                    if (message.PeerId.Value != MainMakara)
                    {
                        @params.PeerId = MainMakara;
                        @params.RandomId = random.Next();
                        @params.Message = operinfoupdate + " - эту инфу я получил от [id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]";
                        Send(@params, client);
                    }
                    return;
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((((text.StartsWith("вышел") || text.StartsWith("ушёл") || text.StartsWith("ушел") || text.StartsWith("ушла")
                || text.StartsWith("ушли") || text.StartsWith("вышли") || text.StartsWith("вышла")) &&
                text.Length < 15 && text.Contains("?") == false && text.Contains("не ") == false && text.Contains("нет") == false)
                || text == "вернулся") && Regex.Replace(text, @"[^\d]+", "").Length < 5 && message.Payload == null && text.Length < 15)
                return true;
            return false;
        }
    }
}
