﻿using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class DeleteKeyboard : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            var buttons = new List<List<MessageKeyboardButton>> { }; // пустой массив кнопок. Без массива не отправляется клавиатура!!

            var keyboard = new MessageKeyboard
            {
                Inline = false,
                OneTime = false,
                Buttons = buttons
            };
            @params.Keyboard = keyboard;
            @params.Message = "Это сообщение должно убрать кнопки";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("наряд закончился") || text.Contains("убери кнопки") || text.Contains("убрать кнопки"))
                return true;
            return false;
        }
    }
}
