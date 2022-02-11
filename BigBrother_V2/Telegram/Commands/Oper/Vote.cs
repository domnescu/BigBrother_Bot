using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    class VoteTelegram : CommandTelegram
    {
        public override string Name => "Голосование за нового опера";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            UserTelegram user = new UserTelegram(message);
            Database db = new Database();
            string text = " " + message.Text.ToLower().Replace("опер", "");
            int NrOfVotes = db.GetNrOfElements("Votes");
            if (NrOfVotes <= 5 && db.GetWorkingVariable("VoteAcces") == "open" && message.ForwardFrom == null && db.CheckText(text, "WarningList"))
            {
                Dictionary<string, string> OperList = db.GetDictionaryString("WarningList");
                if (db.CheckInt64(user.Id, "Votes"))
                {

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: user.FirstName + ", ты уже голосовал/голосовала",
                        cancellationToken: cancellationToken
                    );
                    return;
                }
                string oper = null;
                foreach (var _oper in OperList)
                {
                    if (text.Contains(_oper.Key.ToLower()))
                    {
                        if (_oper.Value == "опер" && _oper.Key != "опер")
                            oper = _oper.Key;
                        else
                        {
                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "А я считаю что " + _oper.Key + " это " + _oper.Value + ", а не опер.",
                                cancellationToken: cancellationToken
                            );
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
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: user.FirstName + " считает что должен заступить " + oper,
                                    cancellationToken: cancellationToken
                                );
                                break;
                            }
                        case 5:
                            {
                                string NewOper = db.WhoIsNewOper();
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: NewOper + " заступил опером.",
                                    cancellationToken: cancellationToken
                                );
                                db.CleanTable("Votes");
                                db.SetWorkingVariable("CurrentOper", NewOper);
                                db.SetWorkingVariable("VoteAcces", "closed");
                                db.InfoUpdate("опер", "После того как заступил " + NewOper + ", я инфы не получал.");
                                break;
                            }
                        default:
                            {
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: user.FirstName + " считает что должен заступить " + oper,
                                    cancellationToken: cancellationToken
                                );
                                break;
                            }
                    }
                }
            }
            else if (message.ForwardFrom == null && db.CheckText(text, "WarningList"))
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Голосование закрыто",
                    cancellationToken: cancellationToken
                );
            }
            else if (message.ForwardFrom != null)
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: user.FirstName + ", мне показалось или кто-то попытался меня обмануть?",
                    cancellationToken: cancellationToken
                );
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if ((text.StartsWith("сказать") || (text.Contains("опер") && text.Contains("где") == false && text.Contains("кто") == false && text.Length < 16
                && db.CheckText(text, "PossibleLocations") == false && text.Contains("номер") == false)) && text.Contains("?") == false)
                return true;
            return false;
        }
    }
}
