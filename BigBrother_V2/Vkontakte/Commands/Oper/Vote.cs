using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class Vote : Command
    {
        public override string Name => "Голосование за нового опера";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            Database db = new();
            @params.Attachments = null;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            string text = " " + message.Text.ToLower().Replace("опер", "");
            int NrOfVotes = db.GetNrOfElements("Votes");
            if (NrOfVotes <= 5 && db.GetWorkingVariable("VoteAcces") == "open" && message.Type != null && db.CheckText(text, "WarningList"))
            {
                Dictionary<string, string> OperList = db.GetDictionaryString("WarningList");
                if (db.CheckInt64(user.Id, "Votes"))
                {
                    //временная переменная для формирования "красивого" ответа
                    string temp = user.Sex == VkNet.Enums.Sex.Male ? "голосовал" : "голосовала";
                    //проверка пола голосовавшего пользователя

                    @params.Message = user.FirstName + ", ты уже " + temp;
                    Send(@params, client);
                    return;
                }
                string oper = null;
                foreach (KeyValuePair<string, string> _oper in OperList)
                {
                    if (text.Contains(_oper.Key.ToLower()))
                    {
                        if (_oper.Value == "опер" && _oper.Key != "опер")
                        {
                            oper = _oper.Key;
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
                                Audio audio = new()
                                {
                                    OwnerId = -187905748,
                                    Id = 456239030
                                };
                                @params.Attachments = new[] { audio };
                                @params.Message = user.FirstName + " считает что должен заступить " + oper;
                                break;
                            }
                        case 3:
                            {
                                if (db.FastFinishVote(oper))
                                {
                                    @params.Message ="Ладно, раз уж вы единогласно голосуете за то что опер " + SaveNewOper(db) + ", то я буду считать что он новый опер.";
                                }
                                break;
                            }
                        case 5:
                            {
                                @params.Message = SaveNewOper(db) + " заступил опером.";
                                break;
                            }
                        default:
                            {

                                @params.Message = user.FirstName + " считает что должен заступить " + oper;
                                break;
                            }
                    }
                    Send(@params, client);
                }
            }
            else if (message.Type != null && db.CheckText(text, "WarningList"))
            {
                @params.Message = "Голосование закрыто";
                Send(@params, client);
            }
            else if (message.Type == null)
            {
                @params.Message = user.FirstName + ", я не обрабатываю голоса из пересланных сообщений";
                Send(@params, client);
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.StartsWith("сказать") || (text.Contains("опер") && text.Contains("где") == false && text.Contains("кто") == false && text.Length < 16
                && db.CheckText(text, "PossibleLocations") == false && text.Contains("номер") == false)) && text.Contains('?') == false;
        }


        private static string SaveNewOper(Database db)
        {
            string newOper = db.WhoIsNewOper();
            db.CleanTable("Votes");
            db.SetWorkingVariable("CurrentOper", newOper);
            db.SetWorkingVariable("VoteAcces", "closed");
            db.InfoUpdate("опер", "После того как заступил " + newOper + ", я инфы не получал.");
            return newOper;
        }
    }
}
