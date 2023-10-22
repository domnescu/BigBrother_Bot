using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class DatabaseUpdate : Command
    {

        public override string Name => "Работа с базой данных";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            string answer;
            if (user.IsAdmin && message.Type == VkNet.Enums.MessageType.Received)
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
                                            : message.Text.ToLower().StartsWith("create") && !Succes
                                                                ? "При создании таблицы возникла ошибка."
                                                                : message.Text.ToLower().StartsWith("alter") && Succes
                                                                                    ? "Обновление таблицы успешно завершено."
                                                                                    : "Что-то пошло не так. Посмотри, может где-то есть очепятка.";
                }
            }
            else
            {
                answer = message.Type == null
                    ? "Если бы я выполнял администраторские команды из пересланных сообщений, вы бы мне испаганили БД. Так что ну вас Нахрен, лучше свяжитесь с админом."
                    : "Хорошая попытка, но НЕТ. Ничего не поменялось от твоего сообщения";
            }
            @params.Message = answer;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.StartsWith("insert") || text.StartsWith("update") || text.StartsWith("delete") || text.StartsWith("create") || text.StartsWith("alter");
        }
    }
}
