﻿//using System;
//using System.Collections.Generic;
//using VkNet;
//using VkNet.Enums.SafetyEnums;
//using VkNet.Model;
//using VkNet.Model.Keyboard;
//using VkNet.Model.RequestParams;

//namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
//{
//    class ButtonsForUser : Command
//    {
//        public override string Name => "Кнопки";

//        MessagesSendParams @params = new MessagesSendParams();

//        public override void Execute(Message message, VkApi client)
//        {
//            var buttons = new List<List<MessageKeyboardButton>> { };
//            if (message.Text.ToLower().Contains("опер"))
//            {

//                buttons = new List<List<MessageKeyboardButton>>
//                        {
//                            new List<MessageKeyboardButton>()
//                            {
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Где опер?"
//                                    },
//                                    Color = KeyboardButtonColor.Primary
//                                },
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Кто опер?"
//                                    },
//                                    Color = KeyboardButtonColor.Primary
//                                }
//                            },
//                            new List<MessageKeyboardButton>()
//                            {
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Убрать кнопки"
//                                    },
//                                    Color = KeyboardButtonColor.Default
//                                }
//                            }
//                        };
//                @params.Message = "Если ты в наряде, лучше используй напиши мне сообщение в котором будет слово \"наряд\" и твоя рота, ну или ПО/ПД если ты в наряде не в роте";

//            }
//            else if (message.Text.ToLower().Contains("еда"))
//            {

//                buttons = new List<List<MessageKeyboardButton>>
//                        {
//                            new List<MessageKeyboardButton>()
//                            {
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Завтрак",
//                                        Payload = "{\"caffeteria\":\"morning\"}"
//                                    },
//                                    Color = KeyboardButtonColor.Primary
//                                },
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Обед",
//                                        Payload = "{\"caffeteria\":\"day\"}"
//                                    },
//                                    Color = KeyboardButtonColor.Primary
//                                },
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Ужин",
//                                        Payload = "{\"caffeteria\":\"eavning\"}"
//                                    },
//                                    Color = KeyboardButtonColor.Primary
//                                }
//                            },
//                            new List<MessageKeyboardButton>()
//                            {
//                                new MessageKeyboardButton
//                                {
//                                    Action = new MessageKeyboardButtonAction
//                                    {
//                                        Type = KeyboardButtonActionType.Text,
//                                        Label = "Убрать кнопки"
//                                    },
//                                    Color = KeyboardButtonColor.Default
//                                }
//                            }
//                        };
//                @params.Message = "Я надеюсь не надо объяснять как они работают.";
//            }
//            var keyboard1 = new MessageKeyboard
//            {
//                Inline = false,
//                OneTime = false,
//                Buttons = buttons
//            };
//            @params.Keyboard = keyboard1;
//            @params.PeerId = message.PeerId.Value;
//            @params.RandomId = new Random().Next();
//            Send(@params, client);
//        }

//        public override bool Contatins(Message message)
//        {
//            string text = message.Text.ToLower();
//            if (message.PeerId.Value < 2000000000 && text.StartsWith("кнопки"))
//                return true;
//            return false;
//        }
//    }
//}
