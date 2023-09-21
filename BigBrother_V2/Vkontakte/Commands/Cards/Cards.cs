using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Cards
{
    class Cards : Command
    {
        public override string Name => "Карты Магазинов";

        public override void Execute(Message message, VkApi client)
        {
            string text = message.Text.ToLower();
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            Database db = new Database();
            if (message.PeerId.Value < 2000000000)
            {
                long PhotoID;
                if (text.Contains("ике") || text.Contains("ike"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "IKEA", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", надеюсь ты купил что-то небольшое, ну или взял себе помошников)";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", Может тебе нужна ещё и помощь чтобы донести покупки ?";
                    }
                    else
                    {
                        @params.Message = "Зачем тебе мебель ?";
                    }
                }
                else if (text.Contains("карусел"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Карусель", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ",можешь купить мне пивка ? ";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ",поделись вкусняшками!";
                    }
                    else
                    {
                        @params.Message = "Существо неопозднанного пола, немедленно покинь здание в котором ты находишься! За тобой уже выехали!";
                    }
                }
                else if (text.Contains("лент"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Лента", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", Хз если ещё работает))";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", сразу предупреждаю, грузчиков в аренду я не предоставляю, тащить будешь сама!";
                    }
                    else
                    {
                        @params.Message = "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                    }
                }
                else if (text.Contains("магнит"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Магнит", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", ты просил карту Магнита ? Получай";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", с тебя вкусняшки! Адрес доставки вкусняшек в ЛС уточни :)";
                    }
                    else
                    {
                        @params.Message = "Хрен знает что ты такое! Бери карту и съебись нахуй отсюда!";
                    }
                }
                else if (text.Contains("оке") || text.Contains("okey"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "ОКЕЙ", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", Окей, держи карту ОКЕЙ";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", ты далеко забрела! Может тебе стоит вернуться обратно ?";
                    }
                    else
                    {
                        @params.Message = "Я по камерам слежу за тобой, немедленно поставь обратно!";
                    }
                }
                else if (text.Contains("перекрест") || text.Contains("перекрёст"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Перекрёсток", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", Ну на тебе карту Перекрёстка";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", Карта перекрёстка, специально для вас.";
                    }
                    else
                    {
                        @params.Message = "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                    }
                }
                else if (text.Contains("пловдив"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Пловдив", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", держи карту Пловдив";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", Карта Пловдив, специально для тебя)";
                    }
                    else
                    {
                        @params.Message = "Ты что там забыло ?";
                    }
                }
                else if (text.Contains("прис") || text.Contains("pris") || text.Contains("приз"))
                {
                    PhotoID = db.GetLong("Cards", "Name", "Prisma", 1);
                    if (user.Sex == VkNet.Enums.Sex.Male)
                    {
                        @params.Message = user.FirstName + ", ты просил карту Призмы? Получай)";
                    }
                    else if (user.Sex == VkNet.Enums.Sex.Female)
                    {
                        @params.Message = user.FirstName + ", тебе ещё нужна карта Призмы ? ";
                    }
                    else
                    {
                        @params.Message = "Существо непонятного пола, уйди из Призмы! Не пугай там людей!";
                    }
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
            if ((text.StartsWith("карт") || (text.Contains("у кого") && text.Contains("есть") && text.Contains("карт"))) && text.Contains("пятёр") == false && text.Contains("пятер") == false)
            {
                return true;
            }

            return false;
        }
    }
}
