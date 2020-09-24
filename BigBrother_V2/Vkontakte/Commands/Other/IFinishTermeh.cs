using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class IFinishTermeh : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            User user = new User(message.PeerId.Value, client);
            bool Succes = db.AddToDB("DELETE FROM WhoKnowTermeh WHERE domain='[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]';");

            if (Succes)
                @params.Message = "Готово, я тебя удалил из списка людей которые могут помочь с теомехом";
            else
                @params.Message = "Так тебя и нет в списке людей которые могут помочь с теормехом";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (((text.Contains("делаю") || text.Contains("знаю")) && (text.Contains("теормех") || text.Contains("механика"))
                && text.Contains("не")) && message.PeerId.Value < 2000000000)
                return true;
            return false;
        }
    }
}
