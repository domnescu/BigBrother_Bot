using System;
using System.Linq;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class LeaveChat : Command
    {
        public override string Name => "Ливнуть из беседы";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            _ = ulong.TryParse(Regex.Replace(message.Text, @"[^\d]+", ""), out ulong peerID);
            try
            {
                ConversationResult chat = client.Messages.GetConversationsById(new[] { (long)peerID });
                @params.Message = "Готово, я успешно покинул беседу " + chat.Items.ElementAt(0).ChatSettings.Title;
                client.Messages.RemoveChatUser(peerID - 2000000000,memberId: -187905748);
            }
            catch
            {
                @params.Message = "Произошла ошибка при попытке покинуть эту беседу";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new Database();
            return text.Contains("покин") && text.Contains("беседу") && Regex.Replace(message.Text, @"[^\d]+", "").Length == 10 && db.CheckText(text, "BotNames");
        }
    }
}
