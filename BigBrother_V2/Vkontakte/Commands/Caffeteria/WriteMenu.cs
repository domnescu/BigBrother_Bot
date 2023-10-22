using System;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Caffeteria
{
    internal class WriteMenu : Command
    {
        public override string Name => "Запись меню столовой в БД";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            VkNet.Utils.VkCollection<GetBannedResult> BlackList = client.Groups.GetBanned(187905748);
            foreach (GetBannedResult BannedUser in BlackList)
            {
                if (BannedUser.Profile.Id == message.FromId.Value)
                {
                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = new Random().Next();
                    @params.Message = db.RandomResponse("RestrictWriteCaffeteria");
                    Send(@params, client);
                    return;
                }
            }
            string text = message.Text.ToLower();
            bool ContainsFood = db.CheckText(text, "CaffeteriaFilter");
            if (ContainsFood && db.CheckText(message.Text.ToLower(), "CaffetetiaFilter2") == false && Regex.Match(text, @"[^a-zA-Zа-яА-ЯёЁ., \t\v\r\n\f)(\\\/-]").Success == false)
            {
                if (text.StartsWith("на завтрак "))
                {
                    db.AddToMenu(text.Replace("на завтрак ", "") + "\nИнформация получена " + DateTime.Now.ToString("dd.MM.yyyy"), "завтрак");
                    @params.Message = "Я запомнил что у вас на завтрак";
                }
                else if (text.StartsWith("на обед "))
                {
                    db.AddToMenu(text.Replace("на обед ", "") + "\nИнформация получена " + DateTime.Now.ToString("dd.MM.yyyy"), "обед");
                    @params.Message = "Теперь я знаю что у вас на обед.";
                }
                else if (text.StartsWith("на ужин "))
                {
                    db.AddToMenu(text.Replace("на ужин ", "") + "\nИнформация получена " + DateTime.Now.ToString("dd.MM.yyyy"), "ужин");
                    @params.Message = "Ну всё, теперь можете спрашивать меня что у вас на ужин :)";
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
                    {
                        db.AddToMenu(text.Replace("сейчас в столовой ", "") + "\nИнформация получена " + DateTime.Now.ToString("dd.MM.yyyy"), time);
                    }
                    else
                    {
                        db.AddToMenu(text.Replace("в столовой ", "") + "\nИнформация получена " + DateTime.Now.ToString("dd.MM.yyyy"), time);
                    }

                    @params.Message = "БлЭт! Надеюсь я ничего не перепутал и все правильно запомнил...у вас же сейчас " + time + "?";
                }
            }
            else
            {
                @params.Message = Regex.Match(text, @"^.*[^A-zА-яЁё].*$").Success
                    ? db.RandomResponse("AltSymbols")
                    : db.CheckText(message.Text.ToLower(), "CaffetetiaFilter2")
                                    ? db.RandomResponse("CaffeteriaAltFilter")
                                    : db.RandomResponse("NotEat");
            }

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            _ = new Database();
            return (text.StartsWith("на завтрак") || text.StartsWith("на обед") || text.StartsWith("на ужин") || text.StartsWith("сейчас в столовой"))
                && text.Contains("форм") == false && text.Contains("кто") == false && text.Contains("есть") == false && text.Contains("?") == false;
        }
    }
}
