using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    class StatusVoteTelegram : CommandTelegram
    {
        public override string Name => "Статус Голосования";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string Text;
            if (db.GetWorkingVariable("VoteAcces") == "open")
            {
                Text = "Статус: открыто \nСписок голосов:\n" + db.GetVoteStatus();
            }
            else
            {
                Text = "Статус: закрыто";
            }

            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: Text,
            cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("статус") && text.Contains("голосования"))
            {
                return true;
            }

            return false;
        }
    }
}
