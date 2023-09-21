using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class ChangeVoteStatus : Command
    {
        public override string Name => "Принудительное изменение опера";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            string Text = message.Text.ToLower();
            Database db = new();
            if (user.IsAdmin && message.Type != null)
            {
                if (Text.Contains("открой"))
                {
                    db.SetWorkingVariable("VoteAcces", "open");
                    @params.Message = "Голосование открыто.";
                }
                else if (Text.Contains("закрой"))
                {
                    db.SetWorkingVariable("VoteAcces", "closed");
                    db.CleanTable("votes");
                    @params.Message = "Голосование закрыто.";
                }

            }
            else
            {
                @params.Message = user.IsAdmin && message.Type == null
                    ? "Пересланное сообщение Админа сообщества ? Серьёзно ? Это так не работает)"
                    : "Только Администраторы сообщества имеют право менять статус голосования";
            }

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("голосование") && db.CheckText(text, "BotNames");
        }
    }
}
