using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    class DisableReactionFor_PushAll : Command
    {
        public override string Name => "Игнорирование All";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            if (message.PeerId.Value > 2000000000)
            {
                var members = client.Messages.GetConversationMembers(message.PeerId.Value);
                foreach (var member in members.Items)
                {
                    if (member.MemberId == message.FromId.Value)
                    {
                        if (member.IsAdmin)
                        {
                            @params.Message = "Окей, в данной беседе, я буду игнорировать @all.";
                            Database db = new Database();
                            db.AddToDB("INSERT INTO IgnoreAll (PeerID) VALUES (" + message.PeerId.Value + ")");
                        }
                        else
                        {
                            @params.Message = "Только Администратор беседы может сказать мне отключить реакцию на @all.";
                        }
                        Send(@params, client);
                    }
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            if (text.Contains("all") && text.Contains("игнорируй") && db.CheckText(text, "BotNames"))
                return true;
            return false;
        }
    }
}
