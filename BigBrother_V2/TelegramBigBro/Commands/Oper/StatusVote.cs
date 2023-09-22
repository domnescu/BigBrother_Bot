using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    internal class StatusVoteTelegram : CommandTelegram
    {
        public override string Name => "Статус Голосования";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            string Text = db.GetWorkingVariable("VoteAcces") == "open" ? "Статус: открыто \nСписок голосов:\n" + db.GetVoteStatus() : "Статус: закрыто";
            _ = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: Text,
            cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("статус") && text.Contains("голосования");
        }
    }
}
