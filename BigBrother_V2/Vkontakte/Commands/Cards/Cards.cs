using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Cards
{
    internal class Cards : Command
    {
        public override string Name => "Карты Магазинов";

        public override void Execute(Message message, VkApi client)
        {
            string text = message.Text.ToLower();
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            Database db = new();
            if (message.PeerId.Value < 2000000000)
            {
                long PhotoID;
                if (text.Contains("ике") || text.Contains("ike"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "IKEA", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", надеюсь ты купил что-то небольшое, ну или взял себе помошников)"
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ", Может тебе нужна ещё и помощь чтобы донести покупки ?"
                            : "Зачем тебе мебель ?";
                }
                else if (text.Contains("карусел"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Карусель", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ",можешь купить мне пивка ? "
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ",поделись вкусняшками!"
                            : "Существо неопозднанного пола, немедленно покинь здание в котором ты находишься! За тобой уже выехали!";
                }
                else if (text.Contains("лент"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Лента", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", Хз если ещё работает))"
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ", сразу предупреждаю, грузчиков в аренду я не предоставляю, тащить будешь сама!"
                            : "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                }
                else if (text.Contains("магнит"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Магнит", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", ты просил карту Магнита ? Получай"
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ", с тебя вкусняшки! Адрес доставки вкусняшек в ЛС уточни :)"
                            : "Хрен знает что ты такое! Бери карту и съебись нахуй отсюда!";
                }
                else if (text.Contains("оке") || text.Contains("okey"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "ОКЕЙ", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", Окей, держи карту ОКЕЙ"
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ", ты далеко забрела! Может тебе стоит вернуться обратно ?"
                            : "Я по камерам слежу за тобой, немедленно поставь обратно!";
                }
                else if (text.Contains("перекрест") || text.Contains("перекрёст"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Перекрёсток", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", Ну на тебе карту Перекрёстка"
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ", Карта перекрёстка, специально для вас."
                            : "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                }
                else if (text.Contains("пловдив"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Пловдив", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", держи карту Пловдив"
                        : user.Sex == VkNet.Enums.Sex.Female ? user.FirstName + ", Карта Пловдив, специально для тебя)" : "Ты что там забыло ?";
                }
                else if (text.Contains("прис") || text.Contains("pris") || text.Contains("приз"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Prisma", 1);
                    @params.Message = user.Sex == VkNet.Enums.Sex.Male
                        ? user.FirstName + ", ты просил карту Призмы? Получай)"
                        : user.Sex == VkNet.Enums.Sex.Female
                            ? user.FirstName + ", тебе ещё нужна карта Призмы ? "
                            : "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                }
                else
                {
                    PhotoID = 457239114;
                }


                Photo photo_attach = new()
                {
                    OwnerId = -187905748,
                    AlbumId = 267692087,
                    Id = PhotoID
                };
                @params.Attachments = new[] { photo_attach };
            }
            else
            {
                @params.Message = user.FirstName + ", карты магазинов доступны только в ЛС.";
            }
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && text.Contains("пятёр") == false && text.Contains("пятер") == false;
        }
    }
}
