using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class WhoIsInCheck : Command
    {
        public override string Name => "Кто в проверке ?";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.FromId.Value, client);
            string Check = db.GetWorkingVariable("WhoIsInCheck");
            if(Check.Contains(" "))
            {
                @params.Message = user.FirstName + ", по общаге ходит проверка в которую входят " + Check + " но это не точно";
            } else
            {
                @params.Message = user.FirstName + ", в общаге щас бушует " + Check + " но это не точно!!!";
            }
            
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("кто") && text.Contains("проверке") && text.Contains("заступ") == false && text.Contains("будет") == false && text.Contains("завтра") == false)
            {
                return true;
            }

            return false;
        }
    }
}