using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    class InviteLink : Command
    {
        public override string Name => "Получение пригласительной ссылки в последний чат откуда поступала инфа по угрозам";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            ulong.TryParse(db.GetWorkingVariable("PeerForAnihilation"), out ulong AnihilationPeerID);
            string Link = client.Messages.GetInviteLink(AnihilationPeerID, false);
            @params.PeerId = message.PeerId;
            @params.RandomId = new Random().Next();
            @params.Message = "Пожалуйста, пригласительная ссылка в беседу из которой я в последний раз получал инфу по оперу \n" + Link;
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if (text.Contains("пригласительная") && text.Contains("ссылка") && db.CheckText(text, "BotNames"))
            {
                return true;
            }

            return false;
        }
    }
}
