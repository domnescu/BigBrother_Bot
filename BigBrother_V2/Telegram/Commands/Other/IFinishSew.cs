using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class IFinishSewTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string response;
            bool Succes = db.AddToDB("DELETE FROM WhoSew WHERE domain='@" + message.From.Username + "';");

            response = Succes ? "Хорошо, я запомнил что ты больше не шьёшь." : "Так тебя и небыло в списке людей которые умеют шить";
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return ((text.Contains("могу") && text.Contains(" шить")) || text.Contains("шью")) && text.Contains("не");
        }
    }
}
