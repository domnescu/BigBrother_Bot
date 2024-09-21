using BigBrother_V2.Additional;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class NewInfoAboutOper : Command
    {
        public override string Name => "Новая информация по оперу";

        private MessagesSendParams @params = new();

        public override async void Execute(Message message, VkApi client)
        {
            @params = new MessagesSendParams();
            Database db = new();
            User user = new(message.FromId.Value, client);
            VkNet.Utils.VkCollection<GetBannedResult> BlackList = client.Groups.GetBanned(187905748);
            foreach (GetBannedResult BannedUser in BlackList)
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
            List<string> PossibleLocations = db.GetListString("PossibleLocations");
            string text = message.Text.ToLower();
            string warningType;
            string operinfoupdate;
            warningType = text.StartsWith("вышел") || text.StartsWith("ушёл") || text.StartsWith("ушел") || text.StartsWith("Вернулся")
                ? "опер"
                : "проверка";
            string lastLocation = db.GetString("WarningList", "Type", warningType, 2);
            if (warningType == "опер")
            {
                warningType = db.GetWorkingVariable("CurrentOper");
            }

            for (int i = 0; i < PossibleLocations.Count; i++)
            {
                if (text.Contains(PossibleLocations[i]))
                {
                    operinfoupdate = warningType + " " + PossibleLocations[i];
                    if (PossibleLocations[i] == "к себе")
                    {
                        operinfoupdate = "По последней информации, " + warningType + " пошёл к себе";
                    }
                    else if (PossibleLocations[i] is "ушёл" or "ушел" or "ушли" or "ушла" or "вышел" or "вышли" or "вышла" or "к себе")
                    {
                        for (int k = 0; k < PossibleLocations.Count; k++)
                        {
                            operinfoupdate = warningType + " " + PossibleLocations[i] + " из ";
                            if (((text.Contains(PossibleLocations[k]) || lastLocation.Contains(PossibleLocations[k]))
                                && PossibleLocations[k] != PossibleLocations[i])&& db.GetString("PossibleLocations", "Location", PossibleLocations[k],3) == "yes")
                            {
                                operinfoupdate += PossibleLocations[k];
                                break;
                            }
                            if (k == PossibleLocations.Count - 1)
                            {
                                operinfoupdate = warningType + " находится непонятно где!";
                            }
                        }
                    }
                    db.InfoUpdate(warningType, operinfoupdate + "\nВремя получения информации " + DateTime.Now.ToShortTimeString());
                    db.SetWorkingVariable("PeerForAnihilation", message.PeerId.Value.ToString());
                    Random random = new();
                    @params.DisableMentions = true;
                    @params.Message = operinfoupdate + "\nэту информацию я получил из ВК от ";
                    StringForLink @string = new()
                    {
                        VK = "[id" + user.Id + "|" + user.FirstNameGen + " " + user.LastNameGen + "]",
                        Telegram = "<a href=\"https://vk.com/" + user.Domain + "\">" + user.FirstNameGen + " " + user.LastNameGen + "</a>"
                    };
                    await MessageDistribution(@params, client, @string);
                    @params.UserIds = null;
                    @params.Message = "Вот что я запомнил:\n" + operinfoupdate;
                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = random.Next();
                    Send(@params, client);
                    List<long> MainMakaraChats = db.GetListLong("MainMakara");
                    foreach (long MainMakara in MainMakaraChats)
                    {
                        if (message.PeerId.Value != MainMakara)
                        {
                            @params.PeerId = MainMakara;
                            @params.RandomId = random.Next();
                            @params.Message = operinfoupdate + "\nэту инфу я получил из ВК от " + @string.VK;
                            Send(@params, client);
                        }
                    }
                    return;
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (((text.StartsWith("вышел") || text.StartsWith("ушёл") || text.StartsWith("ушел") || text.StartsWith("ушла")
                || text.StartsWith("ушли") || text.StartsWith("вышли") || text.StartsWith("вышла")) &&
                text.Length < 15 && text.Contains("?") == false && text.Contains("не ") == false && text.Contains("нет") == false)
                || text == "вернулся") && Regex.Replace(text, @"[^\d]+", "").Length < 5 && message.Payload == null && text.Length < 15;
        }
    }
}
