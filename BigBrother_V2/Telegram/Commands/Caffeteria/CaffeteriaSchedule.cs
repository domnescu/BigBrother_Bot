using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Caffeteria
{
    internal class CaffeteriaScheduleTelegram : CommandTelegram
    {

        public override string Name => "Расписание столовой";


        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            //@params.Message = "Держи, только учти что возможны изменения.";
            _ = await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: "https://sun9-7.userapi.com/impg/fpR-T2LjMlbZCXNitEREZsZS-TEcevYQwE1UAA/8q0l6hsJxjc.jpg?size=1200x1600&quality=96&sign=2ea8c77cf6e9ee05460d8fada794d2dd&type=album",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string Text = message.Text.ToLower();
            return ((Text.Contains("во сколько") || Text.Contains("до скольки") || Text.Contains("когда")) && (Text.Contains("завтрак") || Text.Contains("обед") || Text.Contains("ужин"))) ||
                   (Text.Contains("расписание") && (Text.Contains("столов") || Text.Contains("столов") || Text.Contains("рестора")));
        }
    }
}
