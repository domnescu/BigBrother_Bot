using System;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    internal class GetInviteLinkToChat : Command
    {
        public override string Name => "Получение пригласительной ссылки в беседу с указанным идентификатором";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            if(user.IsAdmin && message.Type != null)
            {
                try
                {
                    _ = ulong.TryParse(Regex.Replace(message.Text, @"[^\d]+", ""), out ulong peerID);
                    string Link = client.Messages.GetInviteLink(peerID, false);
                    @params.Message = "Пожалуйста, ссылка для вступление в беседу с указанным идентификатором " + Link;
                } catch {
                    @params.Message = "Я не могу получить пригласительную ссылку в беседу с указанным идентификатором";
                }
            } else
            {

                @params.Message = message.Type != null
                    ? "Пересланное сообщение с администраторской командой ? Ты щас серьёзно надеялся что я выполню эту команду ?."
                    : "Эта команда доступна только администраторам, либо свяжись отправь сообщение с текстом \"бб вызывай администратора\"";
            }
            @params.RandomId = new Random().Next();
            @params.PeerId = message.PeerId.Value;
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.StartsWith("peer_id=");
        }
    }
}
