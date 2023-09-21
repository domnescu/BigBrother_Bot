using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class TempOper : Command
    {
        public override string Name => "Добавление нового опера";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                string temporarText = message.Text.Remove(0, 11);
                _ = db.AddToDB("INSET INTO WarningList ('warning','type') VALUES ('" + temporarText + "','опер');");
                @params.Message = "Готово, " + temporarText + " добавлен в базу данных как новый опер.";
            }
            else
            {
                @params.Message = message.Type != null
                    ? "Администраторские команды не выполняются из пересланных сообщений."
                    : "У тебя нет прав на выполнение данной команды. Свяжись с одним из администраторов для выполнения этой команды.";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.StartsWith("новый опер");
        }
    }
}
