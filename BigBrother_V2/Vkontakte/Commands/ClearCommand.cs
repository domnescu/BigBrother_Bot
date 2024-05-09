using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    internal class ClearCommand : Command
    {
        public override string Name => "Тестовая команда";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();

            GetConversationMembersResult UsersInChat = client.Messages.GetConversationMembers(message.PeerId.Value);
            Console.ReadLine();
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("test");
        }
    }
}
