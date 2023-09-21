using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram
{
    internal class UserTelegram
    {

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Короткая ссылка на страницу пользователя
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Является ли пользователь Администратором сообщества
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Имя и Фамилия пользователя
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Пользователь является ботом
        /// </summary>
        public bool IsBot { get; set; }
        public UserTelegram(Message message)
        {
            FirstName = message.From.FirstName;
            LastName = message.From.LastName;
            Domain = message.From.Username;
            Id = message.From.Id;
            if (Id == 312191379)
            {
                IsAdmin = true;
            }

            FullName = FirstName + " " + LastName;
            IsBot = message.From.IsBot;

        }
    }
}
