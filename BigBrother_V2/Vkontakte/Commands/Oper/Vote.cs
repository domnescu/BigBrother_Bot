using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    class Vote : Command
    {
        public override string Name => "Голосование за нового опера";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            User user = new User(message.FromId.Value, client);
            Database db = new Database();
            @params.Attachments = null;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            int NrOfVotes = db.GetNrOfElements("Votes");
            if (NrOfVotes <= 5 && db.GetWorkingVariable("VoteAcces") == "open" && message.Type != null)
            {
                Dictionary<string, string> OperList = db.GetDictionaryString("WarningList");
                if (db.CheckInt64(user.Id, "Votes"))
                {
                    //временная переменная для формирования "красивого" ответа
                    string temp;
                    //проверка пола голосовавшего пользователя
                    if (user.Sex == VkNet.Enums.Sex.Male)
                        temp = "голосовал";
                    else
                        temp = "голосовала";
                    @params.Message = user.FirstName + ", ты уже " + temp;
                    Send(@params, client);
                    return;
                }
                string oper = null;
                foreach (var _oper in OperList)
                {
                    if (message.Text.ToLower().Contains(_oper.Key.ToLower()))
                    {
                        if (_oper.Value == "опер" && _oper.Key != "опер")
                            oper = _oper.Key;
                        else if (_oper.Key == "опер")
                        {
                            continue;
                        }
                        else
                        {
                            @params.Message = "А я считаю что " + _oper.Key + " это " + _oper.Value + ", а не опер.";
                            Send(@params, client);
                            return;
                        }
                    }
                }
                if (oper != null)
                {
                    //если пользователь ещё не голосовал, добавляем его в список голосовавших
                    db.AddVote(user.Id, oper);
                    NrOfVotes++;
                    switch (NrOfVotes)
                    {
                        case 1:
                            {
                                Audio audio = new Audio
                                {
                                    OwnerId = -187905748,
                                    Id = 456239030
                                };
                                @params.Attachments = new[] { audio };
                                @params.Message = user.FirstName + " считает что должен заступить " + oper;
                                Send(@params, client);
                                break;
                            }
                        case 5:
                            {
                                string NewOper = db.WhoIsNewOper();
                                @params.Message = NewOper + " заступил опером.";
                                Send(@params, client);
                                db.CleanTable("Votes");
                                db.SetWorkingVariable("CurrentOper", NewOper);
                                db.SetWorkingVariable("VoteAcces", "closed");
                                db.InfoUpdate("опер", "После того как заступил " + NewOper + ", я инфы не получал.");
                                break;
                            }
                        default:
                            {

                                @params.Message = user.FirstName + " считает что должен заступить " + oper;
                                Send(@params, client);
                                break;
                            }
                    }
                }
            }
            else if (message.Type != null)
            {
                @params.Message = "Голосование закрыто";
                Send(@params, client);
            }
            else
            {
                @params.Message = user.FirstName + ", мне показалось или кто-то попытался меня обмануть?";
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if ((text.StartsWith("сказать") || (text.Contains("опер") && text.Contains("где") == false && text.Contains("кто") == false && text.Length < 16 && db.CheckText(text, "PossibleLocations") == false && text.Contains("номер") == false))
                && db.CheckText(text, "WarningList") && text.Contains("?") == false)
                return true;
            return false;
        }
    }
}
