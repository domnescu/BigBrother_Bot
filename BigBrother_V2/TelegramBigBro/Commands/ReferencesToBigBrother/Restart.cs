using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    internal class RestartTelegram : CommandTelegram
    {
        public override string Name => "Перезагрузка";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            UserTelegram user = new(message);
            string response;
            if (user.IsAdmin && message.ForwardFrom == null)
            {
                response = "Моя остановочка)))";
                new Thread(() => { Thread.Sleep(2000); Environment.Exit(0); }).Start();

            }
            else
            {
                response = message.ForwardFrom != null ? "Лучше свяжитесь с администратором." : "Ты не сможешь меня остановить 😈";
            }

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            //Добавить упоминания бота
            return text.Contains("перезагруз") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}