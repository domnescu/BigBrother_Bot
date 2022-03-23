using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class IFinishPaint : Command
    {
        public override string Name => "Пустая Команда";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            bool Succes = db.AddToDB("DELETE FROM WhoPaint WHERE domain='[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]';");
            if (Succes)
            {
                @params.Message = "Готово, я тебя удалил из списка людей которые могут сделать начерт или инжеграф";
            }
            else
            {
                @params.Message = "Так тебя и нет в списке людей которые могут сделать начерт или инжеграф";
            }

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.Contains("не") && text.Contains("делаю") && ((text.Contains("инженерк") || text.Contains("инжеграф") || (text.Contains("инженер") && text.Contains("граф"))) ||
                text.Contains("начерт"))) && message.PeerId.Value < 2000000000)
            {
                return true;
            }

            return false;
        }
    }
}
