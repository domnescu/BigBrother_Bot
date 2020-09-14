using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class SavePeerID : Command
    {
        public override string Name => "Сохранение идентификатора диалога/беседы";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new Database();
            bool Succes = database.AddChat(message.PeerId.Value);
            if (message.PeerId.Value < 2000000000)
            {
                var buttons = new List<List<MessageKeyboardButton>>
                    {
                        new List<MessageKeyboardButton>()
                        {
                            new MessageKeyboardButton
                            {
                                Action = new MessageKeyboardButtonAction
                                {
                                    Type = KeyboardButtonActionType.Text,
                                    Label = "прекрати пересылать инфу"
                                },
                                Color = KeyboardButtonColor.Negative
                            }
                        }
                    };
                @params.Keyboard = new MessageKeyboard
                {
                    Inline = false,
                    OneTime = false,
                    Buttons = buttons
                };
                if (Succes)
                    @params.Message = "Хорошо, я буду присылать тебе всю информацию по оперу";
                else
                    @params.Message = "Ты уже есть в моей базе данных. Если тебе не приходит информация, вызови администратора.";
            }
            else
            {
                @params.Keyboard = null;
                if (Succes)
                    @params.Message = "Ваша беседа успешно добавлена в базу данных";
                else
                    @params.Message = "Ваша беседа уже есть в моей базе данных, если по каким-то причинам информация сюда не приходит, вызовите администратора.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            Database db = new Database();
            string text = message.Text.ToLower();
            if ((text.Contains("пересылай") || text.Contains("присылай")) && text.Contains("инфу") && (message.PeerId.Value < 2000000000 || db.CheckText(text, "BotNames")))
                return true;
            return false;
        }
    }
}
