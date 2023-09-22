using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Cards
{
    internal class Karta5Telegram : CommandTelegram
    {
        public override string Name => "Карта Пятёрочки";


        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: "https://sun9-77.userapi.com/impg/eqLb1UHLiKRXBiVChkWOPEI9LF80x2nrYILypA/A7oSJuyslUU.jpg?size=750x1334&quality=96&sign=c852c18034668b7710bef511a57211ea&type=album",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && (text.Contains("пятёр") || text.Contains("пятер"));
        }
    }
}
