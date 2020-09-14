using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Caffeteria
{
    class ReadMenu:Command
    {
        public override string Name => "Чтение меню столовой из БД";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            string text = message.Text.ToLower();
            string answer = null;
            if (((text.StartsWith("что") || text.StartsWith("чё")) && text.Contains("завтрак")) || message.Payload== "{\"caffeteria\":\"morning\"}")
            {
                answer = db.GetMenu("завтрак");
            }else if (((text.StartsWith("что") || text.StartsWith("чё")) && text.Contains("обед")) || message.Payload == "{\"caffeteria\":\"day\"}")
            {
                answer = db.GetMenu("обед");
            }else if (((text.StartsWith("что") || text.StartsWith("чё")) && text.Contains("ужин")) || message.Payload == "{\"caffeteria\":\"eavning\"}")
            {
                answer = db.GetMenu("ужин");
            }else if ((text.StartsWith("что") || text.StartsWith("чем") || text.StartsWith("чё") || text.StartsWith("че")) && (text.Contains("столов") || text.Contains("рестора") || text.Contains("кормят")))
            {
                int hour = DateTime.Now.Hour;
                if (hour - 1 <= 8 && hour + 1 >= 8)
                {
                    answer = db.GetMenu("завтрак");
                }
                else if (hour - 2 <= 12 && hour + 3 >= 12)
                {
                    answer = db.GetMenu("обед");
                }
                else if (hour - 3 <= 18 && hour + 1 >= 18)
                {
                    answer = db.GetMenu("ужин");
                }
            }
            if (answer == null)
            {
                @params.Message = db.RandomResponse("RandomCaffeteria");
            }
            else
                @params.Message = "Я, конечно, могу ошибаться, но, кажется, в столовой:\n" + answer;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (((text.StartsWith("что") || text.StartsWith("чем") || text.StartsWith("чё") || text.StartsWith("че")) && ((text.Contains("столов") 
                || text.Contains("рестора") || text.Contains("кормят")) || text.Contains("завтрак") || text.Contains("обед") || text.Contains("ужин"))) || message.Payload!=null && message.Payload.Contains("caffeteria"))
                return true;
            return false;
        }
    }
}
