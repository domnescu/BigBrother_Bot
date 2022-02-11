using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Other
{
    class DatabaseUpdateTelegram : CommandTelegram
    {

        public override string Name => "Работа с базой данных";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            UserTelegram user = new UserTelegram(message);
            string answer;
            if (user.IsAdmin && message.ForwardFrom == null)
            {
                Database database = new Database();
                bool Succes = database.AddToDB(message.Text);
                if (message.Text.ToLower().StartsWith("insert") && Succes)
                    answer = "Элемент успешно добавлен в базу данных.";
                else if (message.Text.ToLower().StartsWith("insert") && !Succes)
                    answer = "Произошла ошибка при добавлении в базу данных - проверь правильность ввода!";
                else if (message.Text.ToLower().StartsWith("delete") && Succes)
                    answer = "Элемент успешно удалён из базы данных.";
                else if (message.Text.ToLower().StartsWith("delete") && !Succes)
                    answer = "Произошла ошибка при удалении элемента из базы данных - проверь правильность ввода!";
                else if (message.Text.ToLower().StartsWith("update") && Succes)
                    answer = "Элемент из базы данных успешно обновлён.";
                else if (message.Text.ToLower().StartsWith("update") && !Succes)
                    answer = "Видимо ты где-то ошибся. В базе данных всё осталось в прежнем состоянии.";
                else if (message.Text.ToLower().StartsWith("create") && Succes)
                    answer = "Таблица успешно создана.";
                else
                    answer = "Что-то пошло не так. Посмотри, может где-то есть очепятка.";
            }
            else if (message.ForwardFrom != null)
            {
                answer = "Если бы я выполнял администраторские команды из пересланных сообщений, вы бы мне испаганили БД. Так что ну вас Нахрен, лучше свяжитесь с админом.";
            }
            else
            {
                answer = "Хорошая попытка, но НЕТ. Ничего не поменялось от твоего сообщения";
            }
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: answer,
                cancellationToken: cancellationToken
            );

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.StartsWith("insert") || text.StartsWith("update") || text.StartsWith("delete") || text.StartsWith("create"))
                return true;
            return false;
        }
    }
}
