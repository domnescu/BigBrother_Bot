﻿using System;
using System.Text.RegularExpressions;
using System.Threading;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class TimeOut : Command
    {
        public override string Name => "Включение Тайм-аута";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                if (Regex.Replace(message.Text, @"[^\d]+", "").Length != 0)
                {
                    int time = int.Parse(Regex.Replace(message.Text, @"[^\d]+", ""));
                    db.SetWorkingVariable("TimeOut", (DateTime.Now.Minute + time).ToString());
                    @params.Message = "Спасибо за то что даёте возможность отдохнуть.";

                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = new Random().Next();
                    Send(@params, client);

                    Thread.Sleep(60 * 1000 * time);
                    db.SetWorkingVariable("TimeOut", "0");
                    return;
                }
                else
                {
                    @params.Message = "А ты не хочешь указать на сколько мне нужно отключиться ?";
                }
            }
            else if (message.Type == null)
            {
                @params.Message = "Пересланные сообщения администраторов не обрабатываются.";
            }
            else
            {
                @params.Message = "Данная команда доступна только для администраторов.";
            }

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            Database db = new();
            string text = message.Text.ToLower();
            if ((text.Contains("пауз") || text.Contains("тайм")) && db.CheckText(text, "BotNames"))
            {
                return true;
            }

            return false;
        }
    }
}
