using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class NewOper : Command
    {
        public override string Name => "Кто опер ?";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                string temporarText = message.Text.Remove(0, 11);
                db.AddToDB("INSET INTO WarningList ('warning','type') VALUES ('" + temporarText + "','опер');");
                @params.Message = "Готово, " + temporarText + " добавлен в базу данных как новый опер.";
            }
            else if (message.Type != null)
            {
                @params.Message = "Администраторские команды не выполняются из пересланных сообщений.";
            }
            else
            {
                @params.Message = "У тебя нет прав на выполнение данной команды. Свяжись с одним из администраторов для выполнения этой команды.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.StartsWith("новый опер"))
            {
                return true;
            }

            return false;
        }
    }
}
