using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class Check : Command
    {
        public override string Name => "Кто в проверке ?";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            string text = message.Text.ToLower();
            string answer = "";
            bool Changed = false;
            Dictionary<string, string> WarningList = db.GetDictionaryString("WarningList");

            foreach (KeyValuePair<string, string> warning in WarningList)
            {
                if (text.Contains(warning.Key.ToLower()))
                {
                    if (warning.Value == "проверка")
                    {
                        answer += " " + warning.Key + ",";
                        Changed = true;
                    }
                }
            }
            if (Changed)
            {
                @params.Message = "Хорошо, я запомнил что в проверке " + answer + " но это не точно";
                db.SetWorkingVariable("WhoIsInCheck", answer);
            }
            else
            {
                @params.Message = "Чё за приколы ? Я не смог понять кто в проверке :| либо ты где-то ошибся либо человека торого ты назвал нет в моей базе данных.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("в проверк") && db.CheckText(text, "WarningList");
        }
    }
}
