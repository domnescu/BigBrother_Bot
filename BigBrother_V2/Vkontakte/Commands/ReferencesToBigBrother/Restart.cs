using System;
using System.Threading;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class Restart : Command
    {
        public override string Name => "Перезагрузка";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                Photo photo_attach = new()
                {
                    OwnerId = -187905748,
                    AlbumId = 267692087,
                    Id = 457239026
                };
                @params.Attachments = new[] { photo_attach };
                @params.Message = "Моя остановочка)))";
                new Thread(() => { Thread.Sleep(2000); Environment.Exit(0); }).Start();

            }
            else
            {
                @params.Message = message.Type == null ? "Лучше свяжитесь с администратором." : "Ты не сможешь меня остановить 😈";
            }

            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            //Добавить упоминания бота
            return text.Contains("перезагруз") && db.CheckText(text, "BotNames");
        }
    }
}