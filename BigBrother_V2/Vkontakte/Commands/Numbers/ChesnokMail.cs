using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;


namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    class ChesnokMail : Command
    {
        public override string Name => "Почта Деканата";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            @params.Message = "Почта Чеснокова В.В. - Chesnokovvv@gumrf.ru";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("почта") || text.Contains("у кого")) && text.Contains("почта") && text.Contains("чеснок"))
            {
                return true;
            }

            return false;
        }
    }
}
