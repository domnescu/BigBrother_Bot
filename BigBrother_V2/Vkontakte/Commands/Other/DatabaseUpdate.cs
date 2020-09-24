using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class DatabaseUpdate : Command
    {

        public override string Name => "Работа с базой данных";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            User user = new User(message.FromId.Value, client);
            string answer;
            if (user.IsAdmin && message.Type == VkNet.Enums.MessageType.Received)
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
                else if(message.Text.ToLower().StartsWith("create") && Succes)
                    answer = "Таблица успешно создана.";
                else
                    answer = "Что-то пошло не так. Посмотри, может где-то есть очепятка.";
            }
            else if (message.Type == null)
            {
                answer = "Если бы я выполнял администраторские команды из пересланных сообщений, вы бы мне испаганили БД. Так что ну вас Нахрен, лучше свяжитесь с админом.";
            }
            else
            {
                answer = "Хорошая попытка, но НЕТ. Ничего не поменялось от твоего сообщения";
            }
            @params.Message = answer;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);

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
