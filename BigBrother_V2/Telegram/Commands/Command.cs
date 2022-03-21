using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.TelegramBigBro.Commands
{
    /// <summary>
    /// Абстрактный класс описывающий что должны содержать комманды.
    /// В классах наследуемых от Command комментаррии отсутствуют, т.к. в них нет нужды. Всё что нужно, описано в данном классе.
    /// </summary>
    public abstract class CommandTelegram
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
        public abstract Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken);
        /// <summary>
        /// Метод проверяющий полученное сообщение на соответствие срабатывания команды
        /// </summary>
        /// <param name="message">Сообщение полученное из ВК</param>
        /// <returns>True - если в тексте найдена команда
        /// False - если в тексте не найдена команда</returns>
        public abstract bool Contatins(Message message);

        public async Task MessageDistributionWithTelegram(string text)
        {
            MessagesSendParams @params = new MessagesSendParams();
            @params.Message = text;
            @params.PeerId = null;
            @params.UserIds = null;
            @params.DisableMentions = true;
            Database db = new Database();
            Random rnd = new Random();
            List<long> ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='Telegram'");

            foreach (var ChatID in ListOfConversations)
            {
                Message sentMessage = await Program.botClient.SendTextMessageAsync(
                    chatId: ChatID,
                    text: @params.Message
                );
            }

            ListOfConversations.Clear();
            ListOfConversations = db.GetListLong("Chats", condition: "WHERE Platform='VK'");
            List<long> Users = new List<long>();
            List<long> Chats = new List<long>();
            int count = 1;
            foreach (var peerID in ListOfConversations)
            {
                if (peerID < 2000000000)
                {
                    Users.Add(peerID);
                    count++;
                    if (count == 100)
                    {
                        @params.UserIds = Users;
                        @params.RandomId = rnd.Next();
                        await Program.BotClient.Messages.SendToUserIdsAsync(@params);
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
            await Program.BotClient.Messages.SendToUserIdsAsync(@params);
            foreach (var peerID in Chats)
            {
                @params.RandomId = rnd.Next();
                @params.UserIds = null;
                @params.PeerId = peerID;
                try
                {
                    Program.BotClient.Messages.Send(@params);
                }
                catch
                {
                    db.DeleteChat(peerID);
                }
                
            }
        }
    }
}
