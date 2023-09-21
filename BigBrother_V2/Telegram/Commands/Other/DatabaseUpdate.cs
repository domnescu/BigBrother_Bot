using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Other
{
    internal class DatabaseUpdateTelegram : CommandTelegram
    {

        public override string Name => "Работа с базой данных";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            UserTelegram user = new(message);
            string answer;
            if (user.IsAdmin && message.ForwardFrom == null)
            {
                Database database = new();
                bool Succes = database.AddToDB(message.Text);
                if (message.Text.ToLower().StartsWith("insert") && Succes)
                {
                    answer = "Элемент успешно добавлен в базу данных.";
                }
                else if (message.Text.ToLower().StartsWith("insert") && !Succes)
                {
                    answer = "Произошла ошибка при добавлении в базу данных - проверь правильность ввода!";
                }
                else if (message.Text.ToLower().StartsWith("delete") && Succes)
                {
                    answer = "Элемент успешно удалён из базы данных.";
                }
                else if (message.Text.ToLower().StartsWith("delete") && !Succes)
                {
                    answer = "Произошла ошибка при удалении элемента из базы данных - проверь правильность ввода!";
                }
                else if (message.Text.ToLower().StartsWith("update") && Succes)
                {
                    answer = "Элемент из базы данных успешно обновлён.";
                }
                else
                {
                    answer = message.Text.ToLower().StartsWith("update") && !Succes
                        ? "Видимо ты где-то ошибся. В базе данных всё осталось в прежнем состоянии."
                        : message.Text.ToLower().StartsWith("create") && Succes
                                            ? "Таблица успешно создана."
                                            : "Что-то пошло не так. Посмотри, может где-то есть очепятка.";
                }
            }
            else
            {
                answer = message.ForwardFrom != null
                    ? "Если бы я выполнял администраторские команды из пересланных сообщений, вы бы мне испаганили БД. Так что ну вас Нахрен, лучше свяжитесь с админом."
                    : "Хорошая попытка, но НЕТ. Ничего не поменялось от твоего сообщения";
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: answer,
                cancellationToken: cancellationToken
            );

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.StartsWith("insert") || text.StartsWith("update") || text.StartsWith("delete") || text.StartsWith("create");
        }
    }
}
