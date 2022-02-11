﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    class WhoIsOperTelegram : CommandTelegram
    {
        public override string Name => "Кто опер ?";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new Database();
            UserTelegram user = new UserTelegram(message);
            string oper = db.GetWorkingVariable("CurrentOper");
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: user.FirstName + ", сейчас " + oper + " опер.",
            cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("кто") && text.Contains("опер") && text.Contains("заступ") == false && text.Contains("будет") == false && text.Contains("завтра") == false)
                return true;
            return false;
        }
    }
}
