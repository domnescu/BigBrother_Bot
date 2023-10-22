using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Cards
{
    internal class CardsTelegram : CommandTelegram
    {
        public override string Name => "Карты Магазинов";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            string text = message.Text.ToLower();
            string PhotoID;
            if (text.Contains("ике") || text.Contains("ike"))
            {
                PhotoID = "https://sun9-38.userapi.com/impg/c858220/v858220535/f27d6/v8gGdXiPQdc.jpg?size=539x960&quality=96&sign=96fca5621621635a261aa70a296c07d3&type=album";
            }
            else if (text.Contains("карусел"))
            {
                PhotoID = "https://sun9-34.userapi.com/impg/c858220/v858220535/f27cd/dEZWWpRyZDs.jpg?size=539x960&quality=96&sign=c3fa928b82fe33074f37dc08f69b707d&type=album";
            }
            else if (text.Contains("лент"))
            {
                PhotoID = "https://sun9-13.userapi.com/impg/9ALoV-9gIA9X5WAsF_dLU4-6jdJXWBQf3odWHQ/lJDJVb_znYk.jpg?size=1080x1920&quality=96&sign=75410659a4e8becdb3241ac2efb96701&type=album";
            }
            else if (text.Contains("магнит"))
            {
                PhotoID = "https://sun9-35.userapi.com/impg/3LAwEDI9BoMgLSHEVIUcZxc0QYVPF9ttOTKkTg/pzKH4GyTUIk.jpg?size=800x1600&quality=96&sign=029c34300601b61dd44653fc3ebbe7a4&type=album";
            }
            else
            {
                PhotoID = text.Contains("оке") || text.Contains("okey")
                    ? "https://sun9-29.userapi.com/impg/c858220/v858220535/f27df/L4Hvg0axyIk.jpg?size=539x960&quality=96&sign=8b809971284b8c5c18a3c29191c3d46e&type=album"
                    : text.Contains("перекрест") || text.Contains("перекрёст")
                                    ? "https://sun9-28.userapi.com/impg/c858220/v858220535/f27e9/ViMa9K96twU.jpg?size=750x1334&quality=96&sign=d2bea8c82cd60d4b8948174439cf4b99&type=album"
                                    : text.Contains("пловдив")
                                                    ? "https://sun9-76.userapi.com/impg/pf1Yd66aoOLN4txpZugXi7GG8BrnS2RVCDsBpw/wJL-w3xMNe0.jpg?size=750x1334&quality=96&sign=a8723d4a91e4a4abd694216ce9d9e1ba&type=album"
                                                    : text.Contains("прис") || text.Contains("pris") || text.Contains("приз")
                                                                    ? "https://sun9-84.userapi.com/impg/c858220/v858220535/f27f3/kluAta971VE.jpg?size=720x1280&quality=96&sign=7de2bb38286faff08d4b189e4a09e867&type=album"
                                                                    : "https://sun9-34.userapi.com/impg/kcCwf1565EDBYDfdf8lpEQNoP-e6hOmABN904Q/L8Kc5-k9kwY.jpg?size=356x400&quality=96&sign=a65fd194dfaafc4b8d2e5d9c9b83505b&type=album";
            }

            _ = await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: InputFile.FromUri(PhotoID),
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && text.Contains("пятёр") == false && text.Contains("пятер") == false;
        }
    }
}
