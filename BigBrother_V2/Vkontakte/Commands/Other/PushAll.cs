using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    class PushAll : Command
    {
        public override string Name => "Тестовая команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            User user = new User(message.FromId.Value, client);
            @params.Message = user.FirstName + ", объясни мне пожалуйста, нахуя ты это сделал?";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if (text.Contains("@all") && db.CheckInt64(message.PeerId.Value, "IgnoreAll") == false)
                return true;
            return false;
        }
    }
}
