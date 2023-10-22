using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class ChangeOper : Command
    {
        public override string Name => "Принудительное изменение опера";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            Database db = new();
            if (user.IsAdmin && message.Type != null)
            {
                List<string> operList = db.GetListString("WarningList");
                for (int i = 0; i < operList.Count; i++)
                {
                    if (message.Text.ToLower().Contains(operList[i].ToLower()) && operList[i] != "опер")
                    {
                        db.SetWorkingVariable("CurrentOper", operList[i]);
                        db.SetWorkingVariable("VoteAcces", "closed");
                        db.CleanTable("Votes");
                        db.InfoUpdate("опер", "После того как заступил " + operList[i] + ", я инфы не получал.");

                        @params.Message = "Информация о текущем опере успешно обновлена";
                        break;
                    }
                }
            }
            else
            {
                @params.Message = message.Type == null
                    ? "Для таких \"умников\" как ты, придумали \"защиту ввода\""
                    : "Ты слишком подозрительная личность! Информации от подозрительных личностей, я чёт не особо верю.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("запомни") && text.Contains("опер") && (db.CheckText(text, "BotNames") || message.PeerId.Value < 2000000000);
        }
    }
}
