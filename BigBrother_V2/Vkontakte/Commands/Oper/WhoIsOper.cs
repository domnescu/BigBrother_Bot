using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class WhoIsOper : Command
    {
        public override string Name => "Кто опер ?";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.FromId.Value, client);
            string oper = db.GetWorkingVariable("CurrentOper");
            @params.Message = user.FirstName + ", сейчас " + oper + " опер.";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("кто") && text.Contains("опер") && text.Contains("заступ") == false && text.Contains("будет") == false && text.Contains("завтра") == false)
            {
                return true;
            }

            return false;
        }
    }
}
