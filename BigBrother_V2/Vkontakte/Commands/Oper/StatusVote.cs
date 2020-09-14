using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class StatusVote : Command
    {
        public override string Name => "Статус Голосования";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new Database();
            if (db.GetWorkingVariable("VoteAcces") == "open")
                @params.Message = "Статус: открыто \nСписок голосов:\n" + db.GetVoteStatus();
            else
                @params.Message = "Статус: закрыто";
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("статус") && text.Contains("голосования"))
                return true;
            return false;
        }
    }
}
