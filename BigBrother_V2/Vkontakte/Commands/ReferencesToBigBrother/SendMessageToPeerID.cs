using System.Text.RegularExpressions;
using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.ReferencesToBigBrother
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    internal class SendMessageToPeerID : Command
    {
        public override string Name => "Тестовая команда";

        public override void Execute(Message message, VkApi client)
        {
            MessagesSendParams @params = new();
            User user = new(message.FromId.Value, client);
            if (user.IsAdmin && message.Type != null)
            {
                
                try
                {
                    _ = long.TryParse(message.Text.Substring(16,26), out long peerID);
                    @params.PeerId = peerID;
                    @params.Message = message.Text.Substring(26);
                    @params.RandomId = new Random().Next();
                    Send(@params,client);
                    @params.Message = "Сообщение успешно отправлено в чат с указанным идентификатором";
                }
                catch
                {
                    @params.Message = "Я не могу получить пригласительную ссылку в беседу с указанным идентификатором";
                }
            }
            else
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
            return text.StartsWith("send_to_peer_id=");
        }
    }
}
