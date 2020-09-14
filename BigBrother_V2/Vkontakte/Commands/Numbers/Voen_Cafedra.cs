using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class Voen_CafedraNomber : Command
    {
        public override string Name => "Номер начальника военной кафедры";

        public string Number = "Начальник военной кафедры Алексеев Алексей Сергеевич 8(812)321-15-93";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            @params.Message = Number;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("начальник") && text.Contains("воен")))
                return true;
            return false;
        }
    }
}