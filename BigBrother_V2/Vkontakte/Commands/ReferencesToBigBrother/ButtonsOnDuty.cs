﻿using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class ButtonsOnDuty : Command
    {
        public override string Name => "Кнопки наряд";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            if (message.Text.ToLower().Contains("пд") || message.Text.ToLower().Contains("по"))
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
                                        Label = "У себя",
                                        Payload = "{\"location\":\"опер у себя\"}"
                                    },
                                    Color = KeyboardButtonColor.Primary
                                },
                                new MessageKeyboardButton
                                {
                                    Action = new MessageKeyboardButtonAction
                                    {
                                        Type = KeyboardButtonActionType.Text,
                                        Label = "Вышел",
                                        Payload = "{\"location\":\"опер вышел\"}"
                                    },
                                    Color = KeyboardButtonColor.Primary
                                }
                            },
                            new List<MessageKeyboardButton>()
                            {
                                new MessageKeyboardButton
                                {
                                    Action = new MessageKeyboardButtonAction
                                    {
                                        Type = KeyboardButtonActionType.Text,
                                        Label = "Наряд закончился"
                                    },
                                    Color = KeyboardButtonColor.Default
                                }
                            }
                        };

                MessageKeyboard keyboard1 = new()
                {
                    Inline = false,
                    OneTime = false,
                    Buttons = buttons
                };
                @params.Keyboard = keyboard1;
                @params.Message = "Если возникнут какие-то вопросы, обращайся)";
                @params.PeerId = message.PeerId.Value;
                @params.RandomId = new Random().Next();
                Send(@params, client);
                return;
            }
            List<string> Locations = db.GetListString("PossibleLocations");
            foreach (string location in Locations)
            {
                if (message.Text.ToLower().Contains(location.ToLower()))
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
                                        Label = location,
                                        Payload = "{\"location\":\"опер в "+location+"\"}"
                                    },
                                    Color = KeyboardButtonColor.Primary
                                },
                                new MessageKeyboardButton
                                {
                                    Action = new MessageKeyboardButtonAction
                                    {
                                        Type = KeyboardButtonActionType.Text,
                                        Label = "Вышел",
                                        Payload = "{\"location\":\"опер вышел из "+location+"\"}"
                                    },
                                    Color = KeyboardButtonColor.Primary
                                }
                            },
                            new List<MessageKeyboardButton>()
                            {
                                new MessageKeyboardButton
                                {
                                    Action = new MessageKeyboardButtonAction
                                    {
                                        Type = KeyboardButtonActionType.Text,
                                        Label = "Наряд закончился"
                                    },
                                    Color = KeyboardButtonColor.Default
                                }
                            }
                        };

                    MessageKeyboard keyboard1 = new()
                    {
                        Inline = false,
                        OneTime = false,
                        Buttons = buttons
                    };
                    @params.Keyboard = keyboard1;
                    @params.Message = "Кнопка на которой написано \"" + location + "\" отправляет мне инфу о том что опер в \"" + location + "\"";
                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = new Random().Next();
                    Send(@params, client);
                    break;
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return message.PeerId.Value < 2000000000 && text.Contains("не ") == false && text.Contains("наряд");
        }
    }
}
