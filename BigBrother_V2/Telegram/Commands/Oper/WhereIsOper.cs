using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.Telegram.Commands.Oper
{
    internal class WhereIsOperTelegram : CommandTelegram
    {
        public override string Name => "Где опер?";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string Location;
            Dictionary<string, string> WarningList = db.GetDictionaryString("WarningList");
            foreach (KeyValuePair<string, string> warning in WarningList)
            {
                if (message.Text.ToLower().Contains(warning.Key.ToLower()) || message.Text.ToLower().StartsWith("ень"))
                {
                    Location = db.GetString("WarningList", "Type", warning.Value, 2);
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: Location,
                    cancellationToken: cancellationToken
                    );
                    break;
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("где") && (db.CheckText(text, "WarningList") || text.StartsWith("ень"));
        }
    }
}
