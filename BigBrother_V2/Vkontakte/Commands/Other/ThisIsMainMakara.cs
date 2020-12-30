using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class ThisIsMainMakara : Command
    {

        public override string Name => "Указание на главную беседу Макары.";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new Database();
            User user = new User(message.FromId.Value, client);
            if (message.Type == VkNet.Enums.MessageType.Received && user.IsAdmin)
            {
                if (message.PeerId.Value < 2000000000)
                {
                    database.AddToDB("INSERT INTO MainMakara (PeerID) VALUES (" + message.PeerId +");");
                    database.SetWorkingVariable("MainMakara", message.PeerId.Value.ToString());
                    @params.Message = "Сделано! Теперь я буду знать что это общая беседа Макары";
                }
                @params.Message = "Балбес! Ты мне в личку это пишешь ? серьёзно ? Афигеть ты дурень!";
            }
            else if (message.Type == null)
            {
                @params.Message = "Пересланное сообщение не сработает. А то найдутся всякие дебилы которые будут вводить в меня в заблуждение.";
            }
            else
            {
                @params.Message = "Только администратор сообщества имеет доступ к данной команде.";
            }
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if (text.Contains("запомни") && (text.Contains("главная") || text.Contains("общая")) && text.Contains("беседа") && db.CheckText(text, "BotNames"))
                return true;
            return false;
        }
    }
}
