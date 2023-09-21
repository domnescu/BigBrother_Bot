using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class IPaintTelegram : CommandTelegram
    {
        public override string Name => "Я делаю начерт";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            _ = db.AddToDB("INSERT INTO WhoPaint (domain,Platform) VALUES ('@" + message.From.Username + "','Telegram')");
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Хорошо, я запомнил что ты делаешь начерт или инжеграф",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("не") == false && text.Contains("делаю") && (text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф")) ||
                text.Contains("начерт"));
        }
    }
}
