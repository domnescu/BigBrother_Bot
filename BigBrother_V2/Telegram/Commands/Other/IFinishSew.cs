using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class IFinishSewTelegram : CommandTelegram
    {
        public override string Name => "Пустая Команда";
        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string response;
            bool Succes = db.AddToDB("DELETE FROM WhoSew WHERE domain='@" + message.From.Username + "';");

            if (Succes)
            {
                response = "Хорошо, я запомнил что ты больше не шьёшь.";
            }
            else
            {
                response = "Так тебя и небыло в списке людей которые умеют шить";
            }

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (((text.Contains("могу") && text.Contains(" шить")) || text.Contains("шью")) && text.Contains("не"))
            {
                return true;
            }

            return false;
        }
    }
}
