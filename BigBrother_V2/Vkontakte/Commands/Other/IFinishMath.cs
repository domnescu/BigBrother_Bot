using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class IFinishMath : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            bool Succes = db.AddToDB("DELETE FROM WhoKnowMath WHERE domain='[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]';");

            @params.Message = Succes
                ? "Готово, я тебя удалил из списка людей которые могут помочь с вышматом"
                : "Так тебя и нет в списке людей которые могут помочь с вышматом";

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.Contains("делаю") || text.Contains("знаю") || (text.Contains("могу") && text.Contains("помочь"))) && (text.Contains("матан") || text.Contains("матем") || text.Contains("вышмат"))
                && text.Contains("не") && message.PeerId.Value < 2000000000;
        }
    }
}
