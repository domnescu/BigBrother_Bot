using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class DeleteKeyboard : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            List<List<MessageKeyboardButton>> buttons = new() { }; // пустой массив кнопок. Без массива не отправляется клавиатура!!

            MessageKeyboard keyboard = new()
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
            return text.Contains("наряд закончился") || text.Contains("убери кнопки") || text.Contains("убрать кнопки");
        }
    }
}
