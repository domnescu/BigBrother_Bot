using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class IPrintTelegram : CommandTelegram
    {
        public override string Name => "Я печатаю";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new Database();
            db.AddToDB("INSERT INTO WhoPrint (domain,Platform) VALUES ('@" + message.From.Username + "','Telegram')");
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Хорошо, я запомнил что ты можешь распечатать",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("могу") && text.Contains("печата") && text.Contains("не") == false)
                return true;
            return false;
        }
    }
}
