using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class ChangeOperTelegram : CommandTelegram
    {
        public override string Name => "Принудительное изменение опера";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            UserTelegram user = new(message);
            Database db = new();
            string Response = string.Empty;
            if (user.IsAdmin && message.ForwardFrom == null)
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

                        Response = "Ну допустим запомнил, и что теперь ?";
                        break;
                    }
                }
            }
            else
            {
                Response = message.ForwardFrom != null
                    ? "Для таких \"умников\" как ты, придумали \"защиту ввода\""
                    : "Ты слишком подозрительная личность! Информации от подозрительных личностей, я чёт не особо верю.";
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("запомни") && text.Contains("опер") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
