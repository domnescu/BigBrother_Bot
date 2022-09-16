using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    class ChatList : Command
    {
        public override string Name => "Тестовая команда";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "ID беседы - " + message.PeerId.Value.ToString();
            @params.PeerId = 235052667;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (message.PeerId > 2000000000 && message.FromId == 112613077)
            {
                return true;
            }
            return false;
        }
    }
}
