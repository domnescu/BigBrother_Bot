using BigBrother_V2.Additional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    /// <summary>
    /// Абстрактный класс описывающий что должны содержать комманды.
    /// В классах наследуемых от Command комментаррии отсутствуют, т.к. в них нет нужды. Всё что нужно, описано в данном классе.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Переменная которая хранит название команды
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Метод в котором содержится генерируется ответ.
        /// </summary>
        /// <param name="message">Сообщение полученное из ВК</param>
        /// <param name="client">Авторизованный клиент ВК - от его имени и будет отправлен ответ</param>
        public abstract void Execute(Message message, VkApi client);
        /// <summary>
        /// Метод проверяющий полученное сообщение на соответствие срабатывания команды
        /// </summary>
        /// <param name="message">Сообщение полученное из ВК</param>
        /// <returns>True - если в тексте найдена команда
        /// False - если в тексте не найдена команда</returns>
        public abstract bool Contatins(Message message);
        /// <summary>
        /// Метод отправляющий сообщения в ВК
        /// </summary>
        /// <param name="params">Параметры сообщения (текст,вложения, получатель, RandomId и т.д.) </param>
        /// <param name="client">Авторизованный клиент от имени которого отправляются сообщения.</param>
        public void Send(MessagesSendParams @params, VkApi client)
        {
            _ = client.Messages.Send(@params);
        }
        /// <summary>
        /// Асинхронная отправка сообщений 100 пользователям одновременно.
        /// Асинхронность используется для того, чтобы во время отправки сообщений, одновременно формировался следующий список из 100 человек
        /// </summary>
        /// <param name="params">Параметры сообщения</param>
        /// <param name="client">Клиент через который следует отправить сообщения</param>
        public async Task SendToUsersIds(MessagesSendParams @params, VkApi client)
        {
            _ = await client.Messages.SendToUserIdsAsync(@params);
        }
        /// <summary>
        /// Метод отвечающий за массовые рассылки сообщений
        /// </summary>
        /// <param name="params">Параметры сообщения</param>
        /// <param name="client">Клиент через который следует отправить сообщения</param>
        public async Task MessageDistribution(MessagesSendParams @params, VkApi client, StringForLink @string = null)
        {
            @params.PeerId = null;
            @params.UserIds = null;
            Database db = new();
            Random rnd = new();

            List<long> ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='Telegram'");

            foreach (long ChatID in ListOfConversations)
            {
                Telegram.Bot.Types.Message sentMessage = await Program.botClient.SendTextMessageAsync(
                    chatId: ChatID,
                    text: @params.Message + @string.Telegram,
                    parseMode: ParseMode.Html
                );
            }
            @params.Message += @string.VK;
            ListOfConversations.Clear();
            ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='VK'");
            List<long> Users = new();
            List<long> Chats = new();
            int count = 1;
            foreach (long peerID in ListOfConversations)
            {
                if (peerID < 2000000000)
                {
                    Users.Add(peerID);
                    count++;
                    if (count == 100)
                    {
                        @params.UserIds = Users;
                        @params.RandomId = rnd.Next();
                        await SendToUsersIds(@params, client);
                        count = 1;
                        Users.Clear();
                    }
                }
                else
                {
                    Chats.Add(peerID);
                }
            }
            @params.UserIds = Users;
            @params.RandomId = rnd.Next();
            await SendToUsersIds(@params, client);
            foreach (long peerID in Chats)
            {
                @params.RandomId = rnd.Next();
                @params.UserIds = null;
                @params.PeerId = peerID;
                try
                {
                    Send(@params, client);
                }
                catch
                {
                    _ = db.DeleteChat(peerID);
                }
            }
        }
    }
}
