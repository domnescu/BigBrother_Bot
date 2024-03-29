﻿using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class DeletePeerID : Command
    {
        public override string Name => "Удаление диалога из БД";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new();
            bool Succes = database.DeleteChat(message.PeerId.Value);
            if (message.PeerId.Value < 2000000000)
            {
                if (Succes)
                {
                    List<List<MessageKeyboardButton>> buttons = new()
                    {
                        new List<MessageKeyboardButton>()
                        {
                            new MessageKeyboardButton
                            {
                                Action = new MessageKeyboardButtonAction
                                {
                                    Type = KeyboardButtonActionType.Text,
                                    Label = "Пересылай инфу"
                                },
                                Color = KeyboardButtonColor.Primary
                            }
                        }
                    };
                    @params.Keyboard = new MessageKeyboard
                    {
                        Inline = false,
                        OneTime = false,
                        Buttons = buttons
                    };
                    @params.Message = "Хорошо, не буду больше беспокоить тебя.";
                }
                else
                {
                    @params.Message = "Видимо где-то произошла ошибка, попробуй вызвать Администратора.";
                }
            }
            else
            {
                @params.Keyboard = null;
                @params.Message = Succes
                    ? "Хорошо, не буду больше отправлять вам информацию."
                    : "Что-то пошло не по плану. Попробуйте связаться с Админом сообщества.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            Database db = new();
            string text = message.Text.ToLower();
            return (text.Contains("прекрати") || text.Contains("перестань")) && text.Contains("инф") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames"));
        }
    }
}
