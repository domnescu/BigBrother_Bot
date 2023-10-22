using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class StatusVote : Command
    {
        public override string Name => "Статус Голосования";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            @params.Message = db.GetWorkingVariable("VoteAcces") == "open" ? "Статус: открыто \nСписок голосов:\n" + db.GetVoteStatus() : "Статус: закрыто";

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("статус") && text.Contains("голосования");
        }
    }
}
