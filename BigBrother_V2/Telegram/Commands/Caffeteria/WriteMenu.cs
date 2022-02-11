using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Caffeteria
{
    class WriteMenuTelegram : CommandTelegram
    {
        public override string Name => "Запись меню столовой в БД";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new Database();
            string text = message.Text.ToLower();
            string textToResponse = String.Empty;
            bool ContainsFood = db.CheckText(text, "CaffeteriaFilter");
            if (ContainsFood && db.CheckText(message.Text.ToLower(), "CaffetetiaFilter2") == false && Regex.Match(text, @"[^a-zA-Zа-яА-ЯёЁ., \t\v\r\n\f)(\\\/-]").Success == false)
            {
                if (text.StartsWith("на завтрак "))
                {
                    db.AddToMenu(text.Replace("на завтрак ", ""), "завтрак");
                    textToResponse = "Я запомнил что у вас на завтрак";
                }
                else if (text.StartsWith("на обед "))
                {
                    db.AddToMenu(text.Replace("на обед ", ""), "обед");
                    textToResponse = "Теперь я знаю что у вас на обед.";
                }
                else if (text.StartsWith("на ужин "))
                {
                    db.AddToMenu(text.Replace("на ужин ", ""), "ужин");
                    textToResponse = "Ну всё, теперь можете спрашивать меня что у вас на ужин :)";
                }
                else if (text.StartsWith("сейчас в столовой ") || text.StartsWith("в столовой "))
                {
                    int hour = DateTime.Now.Hour;
                    string time = null;
                    if (hour - 1 <= 8 && hour + 1 >= 8)
                    {
                        time = "завтрак";
                    }
                    else if (hour - 1 <= 12 && hour + 3 >= 12)
                    {
                        time = "обед";
                    }
                    else if (hour - 3 <= 18 && hour + 1 >= 18)
                    {
                        time = "ужин";
                    }
                    if (text.StartsWith(""))
                        db.AddToMenu(text.Replace("сейчас в столовой ", ""), time);
                    else
                        db.AddToMenu(text.Replace("в столовой ", ""), time);
                    textToResponse = "БлЭт! Надеюсь я ничего не перепутал и все правильно запомнил...у вас же сейчас " + time + "?";
                }
            }
            else if (Regex.Match(text, @"^.*[^A-zА-яЁё].*$").Success)
            {
                textToResponse = db.RandomResponse("AltSymbols");
            }
            else if (db.CheckText(message.Text.ToLower(), "CaffetetiaFilter2"))
            {
                textToResponse = db.RandomResponse("CaffeteriaAltFilter");
            }
            else
                textToResponse = db.RandomResponse("NotEat");
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: textToResponse,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("на завтрак") || text.StartsWith("на обед") || text.StartsWith("на ужин") || text.StartsWith("сейчас в столовой"))
                && text.Contains("форм") == false && text.Contains("кто") == false && text.Contains("есть") == false && text.Contains("?") == false)
                return true;
            return false;
        }
    }
}
