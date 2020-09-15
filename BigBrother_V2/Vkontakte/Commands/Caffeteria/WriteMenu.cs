using System;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Caffeteria
{
    class WriteMenu : Command
    {
        public override string Name => "Запись меню столовой в БД";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            string text = message.Text.ToLower();
            bool ContainsFood = db.CheckText(text, "CaffeteriaFilter");
            if (ContainsFood && db.CheckText(message.Text.ToLower(),"CaffetetiaFilter2") && Regex.Match(text, @"[^a-zA-Zа-яА-ЯёЁ., \t\v\r\n\f)(\\\/]").Success==false)
            {
                if (text.StartsWith("на завтрак "))
                {
                    db.AddToMenu(text.Replace("на завтрак ", ""), "завтрак");
                    @params.Message = "Я запомнил что у вас на завтрак";
                }
                else if (text.StartsWith("на обед "))
                {
                    db.AddToMenu(text.Replace("на обед ", ""), "обед");
                    @params.Message = "Теперь я знаю что у вас на обед.";
                }
                else if (text.StartsWith("на ужин "))
                {
                    db.AddToMenu(text.Replace("на ужин ", ""), "ужин");
                    @params.Message = "Ну всё, теперь можете спрашивать меня что у вас на ужин :)";
                }
                else if (text.StartsWith("сейчас в столовой "))
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
                    db.AddToMenu(text.Replace("сейчас в столовой ", ""), time);
                    @params.Message = "БлЭт! Надеюсь я ничего не перепутал и все правильно запомнил...у вас же сейчас " + time + "?";
                }
            } else if (Regex.Match(text, @"^.*[^A-zА-яЁё].*$").Success)
            {
                @params.Message = db.RandomResponse("AltSymbols");
            }
            else if (db.CheckText(message.Text.ToLower(), "CaffetetiaFilter2")==false)
            {
                @params.Message = db.RandomResponse("CaffeteriaAltFilter");
            } else 
                @params.Message = db.RandomResponse("NotEat");
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if ((text.StartsWith("на завтрак") || text.StartsWith("на обед") || text.StartsWith("на ужин") || text.StartsWith("сейчас в столовой"))
                && text.Contains("форм") == false && text.Contains("кто") == false && text.Contains("есть") == false && text.Contains("?") == false)
                return true;
            return false;
        }
    }
}
