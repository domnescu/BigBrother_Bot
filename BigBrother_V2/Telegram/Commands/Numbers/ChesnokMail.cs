﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class ChesnokMailTelegram : CommandTelegram
    {
        public override string Name => "Почта Деканата";


        public string Number = "Почта Чеснокова В.В. - Chesnokovvv@gumrf.ru";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Number,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("почта") || text.Contains("у кого")) && text.Contains("почта") && text.Contains("чеснок"))
                return true;
            return false;
        }
    }
}
