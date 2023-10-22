using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    internal class HospitalScheduleTelegram : CommandTelegram
    {
        public override string Name => "Расписание 64 Поликлиники";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: InputFile.FromUri("https://sun9-12.userapi.com/impg/v9lsOgH5nQMLZQCGR8yrQkXXnLkbceQ4zwFVzw/Ol8SdA21IkA.jpg?size=1920x1440&quality=96&sign=032ca8729e51bf85ddd9962970d32067&type=album"),
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("расписание") && (text.Contains("больницы") || text.Contains("поликлиники"));
        }
    }
}
