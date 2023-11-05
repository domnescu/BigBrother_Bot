using System;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class Anihilation_Protocol : Command
    {
        public override string Name => "Протокол уничтожения беседы";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            Database db = new();
            if (user.IsAdmin && message.Type != null)
            {
                @params.PeerId = message.PeerId.Value;
                @params.Message = "‼ ☑ Протокол анигиляции успешно запущен. ‼ ВНИМАНИЕ во время работы протокола анигиляции могут наблюдаться перебои в работе или увеличение времени ответа на запросы‼";
                @params.RandomId = new Random().Next();
                Send(@params, client);
                _ = long.TryParse(Regex.Replace(message.Text, @"[^\d]+", ""), out long AnihilationPeerID);
                GetConversationMembersResult UsersInChat = client.Messages.GetConversationMembers(AnihilationPeerID);
                for (int i = 0; i < UsersInChat.Count; i++)
                {
                    db.SaveFuckingChat(UsersInChat.Profiles[i].Id, UsersInChat.Items[i].IsAdmin);
                }
                @params.Message = "✅Первый этап заверщён. Список всех участников беседы успешно выгружен в мою базу данных.";
                @params.RandomId = new Random().Next();
                Send(@params, client);
                @params.Message = "☑ Начинаю второй этап протокола анигиляции. Проверяю возможность кикнуть участников беседы.";
                @params.RandomId = new Random().Next();
                int countKickPos = 0;
                Send(@params, client);
                foreach (ConversationMember member in UsersInChat.Items)
                {
                    if (member.CanKick)
                    {
                        countKickPos++;
                    }
                }
                @params.Message = "✅Второй этап протокола анигиляции завершён. По результатам второго этапа была получена информация о возможности кика " + countKickPos.ToString() + " участников";
                @params.RandomId = new Random().Next();
                Send(@params, client);

                @params.PeerId = AnihilationPeerID;
                @params.Message = "Этот бот писался не для того чтобы всякие долбоёбы добавляли его в левые беседы. Я буду уничтожать любую беседу не связанную с Макарой - чтобы до вас блять дошло что этот бот не для вас.";
                @params.RandomId = new Random().Next();
                Send(@params, client);
                @params.PeerId = message.PeerId.Value;
                @params.Message = "☑ Начинаю третий этап протокола анигиляции. Третий этап будет выполняться пока меня не кикнут из беседы или я не кикну всех остальных.";
                @params.RandomId = new Random().Next();
                Send(@params, client);
                int cKick = 0;
                foreach (ConversationMember member in UsersInChat.Items)
                {
                    if (member.CanKick)
                    {
                        _ = client.Messages.RemoveChatUser((ulong)AnihilationPeerID - 2000000000, memberId: member.MemberId);
                        cKick++;
                    }
                }

                @params.Message = "✅Третий этап протокола анигиляции успешно выполнен. Кикнуты " + cKick.ToString() + " человек, в беседе остались " + (UsersInChat.Count - cKick).ToString() + " человек которые у меня по каким-то причинам не получилось кикнуть.";
                @params.RandomId = new Random().Next();
                Send(@params, client);
                @params.Message = "✅Протокол аннигиляции успешно завершился.";
                @params.RandomId = new Random().Next();
                Send(@params, client);
            }
            else
            {
                @params.Message = message.Type == null
                    ? "Протокол анигиляции активируется только личным сообщением от администратора. Пересланные сообщения не действительны."
                    : "Протокол анигиляции недоступен для тебя.";
            }
            @params.DisableMentions = true;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("протокол") && text.Contains("аннигиляция") && Regex.Replace(message.Text, @"[^\d]+", "").Length == 10;
        }
    }
}