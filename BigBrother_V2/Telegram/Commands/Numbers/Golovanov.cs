using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Numbers
{
    internal class GolovanovNomberTelegram : CommandTelegram
    {
        public override string Name => "Номер Голованова";

        public string Number = "Начальник учебной части - заместитель начальника военного учебного центра Головатов Сергей Анатольевич 8(812)321-15-92";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Number,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("голованов");
        }
    }
}