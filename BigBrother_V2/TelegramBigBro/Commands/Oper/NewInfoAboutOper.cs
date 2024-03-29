﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VkNet.Model;

namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    internal class NewInfoAboutOperTelegram : CommandTelegram
    {
        public override string Name => "Новая информация по оперу";

        public override async Task Execute(Telegram.Bot.Types.Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            _ = new UserTelegram(message);
            List<string> PossibleLocations = db.GetListString("PossibleLocations");
            string text = message.Text.ToLower();
            string warningType;
            string operinfoupdate;
            warningType = text.StartsWith("вышел") || text.StartsWith("ушёл") || text.StartsWith("ушел") || text.StartsWith("вернулся")
                ? db.GetWorkingVariable("CurrentOper")
                : "проверка";

            for (int i = 0; i < PossibleLocations.Count; i++)
            {
                if (text.Contains(PossibleLocations[i]))
                {
                    operinfoupdate = warningType + " " + PossibleLocations[i];
                    if (PossibleLocations[i] == "к себе")
                    {
                        operinfoupdate = "По последней информации, " + warningType + " пошёл к себе";
                    }
                    else if (PossibleLocations[i] is "ушёл" or "ушел" or "ушли" or
                        "ушла" or "вышел" or "вышли" or "вышла")
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
                                operinfoupdate = warningType + " находится в состоянии суперпозиции!!";
                            }
                        }
                    }
                    db.InfoUpdate(warningType, operinfoupdate + "\nВремя получения информации " + DateTime.Now.ToShortTimeString());
                    await MessageDistributionWithTelegram(operinfoupdate + "\nэту инфу я получил из Телеграма от @" + message.From.Username);
                    _ = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Вот что я запомнил:\n" + operinfoupdate,
                        cancellationToken: cancellationToken
                    );
                    List<long> MainMakaraChats = db.GetListLong("MainMakara");
                    foreach (long MainMakara in MainMakaraChats)
                    {
                        MessagesSendParams @params = new()
                        {
                            PeerId = MainMakara,
                            RandomId = new Random().Next(),
                            Message = operinfoupdate + "\nэту инфу я получил из Телеграма от @" + message.From.Username
                        };
                        _ = Program.BotClientVK.Messages.Send(@params);
                    }
                    return;
                }
            }
        }

        public override bool Contatins(Telegram.Bot.Types.Message message)
        {
            string text = message.Text.ToLower();
            return (((text.StartsWith("вышел") || text.StartsWith("ушёл") || text.StartsWith("ушел") || text.StartsWith("ушла")
                || text.StartsWith("ушли") || text.StartsWith("вышли") || text.StartsWith("вышла")) &&
                text.Length < 15 && text.Contains("?") == false && text.Contains("не ") == false && text.Contains("нет") == false)
                || text == "вернулся") && Regex.Replace(text, @"[^\d]+", "").Length < 5 && text.Length < 15;
        }
    }
}
