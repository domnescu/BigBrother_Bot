using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class ThisIsMainMakara : Command
    {

        public override string Name => "Указание на главную беседу Макары.";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database database = new();
            User user = new(message.FromId.Value, client);
            if (message.Type == VkNet.Enums.MessageType.Received && user.IsAdmin)
            {
                if (message.PeerId.Value > 2000000000)
                {
                    _ = database.AddToDB("INSERT INTO MainMakara (PeerID) VALUES (" + message.PeerId + ");");
                    database.SetWorkingVariable("MainMakara", message.PeerId.Value.ToString());
                    @params.Message = "Сделано! Теперь я буду знать что это общая беседа Макары";
                }
                else
                {
                    @params.Message = "Балбес! Ты мне в личку это пишешь ? серьёзно ? Афигеть ты дурень!";
                }
            }
            else
            {
                @params.Message = message.Type == null
                    ? "Пересланное сообщение не сработает. А то найдутся всякие дебилы которые будут вводить в меня в заблуждение."
                    : "Только администратор сообщества имеет доступ к данной команде.";
            }
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("запомни") && (text.Contains("главная") || text.Contains("общая")) && text.Contains("беседа") && db.CheckText(text, "BotNames");
        }
    }
}
