using System;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class ShopLink : Command
    {
        public override string Name => "Ссылка на беседу для продажи.";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            Database db = new();
            if (message.PeerId.Value > 2000000000 && message.FromId.Value > 0)
            {
                if (db.KickUser(message.FromId.Value, message.PeerId.Value - 2000000000))
                {
                    GetConversationMembersResult UsersInChat = client.Messages.GetConversationMembers(message.PeerId.Value);
                    for (int i = 0; i < UsersInChat.Count; i++)
                    {
                        if (user.Id == UsersInChat.Items[i].MemberId)
                        {
                            //В будущем может быть сделаю чтобы работало
                            //client.Messages.Delete(conversationMessageIds: new[] { (ulong)message.ConversationMessageId }, (ulong)message.PeerId.Value, deleteForAll: true);

                            if (UsersInChat.Items[i].CanKick == true)
                            {
                                _ = client.Messages.RemoveChatUser((ulong)message.PeerId.Value - 2000000000, message.FromId.Value);
                                @params.Message = "Несите нового! Этот не понял с первого раза!";
                            }
                            else
                            {
                                @params.Message = "Дайте мне права Администатора в беседе! Я хочу кикнуть эту мразь!";
                            }
                        }
                    }
                }
                else
                {
                    _ = client.Messages.Delete(conversationMessageIds: new[] { (ulong)message.ConversationMessageId }, (ulong)message.PeerId.Value, deleteForAll: true);
                    @params.Message = user.FirstName + ", для таких сообщений, есть отдельаня беседа. Если попробуешь ещё раз отправить что-то подобное, я тебя кикну.\n https://vk.me/join/AJQ1d5A_1grjDZ0ArYPhk0rr";
                }
            }
            else if (message.PeerId.Value < 2000000000)
            {
                @params.Message = "Нахрен ты мне в личку эту хрень отправил ? Ты что совсем тупой ?";
            }
            else if (message.FromId.Value < 0 && message.Type == null)
            {
                @params.Message = "Вы блять серьёзно ? Вы написали бота чтобы он отправлял сообщения о продаже ?";
            }
            else if (message.FromId.Value < 0 && message.Type != null)
            {
                @params.Message = "Бляяя...да вы заебали уже! Вам чё сука одной беседы не хватает для продажи своей хуйни ? Вы паблики создаёте, пересылаете посты с их стены...Лучше бы вы так упорно учились!";
            }

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            Regex regex = new(@"([0-9]+.?[ \t\v\r\n\f]?((ру?б?)|(₽+)))+");
            if (message.Attachments.Count == 0)
            {
                string text = message.Text.ToLower();
                MatchCollection matches = regex.Matches(text);
                return matches.Count > 1 && text.Contains("раз") == false && text.Contains("рота") == false;
            }
            else
            {
                string text = message.Text.ToLower();
                MatchCollection matches = regex.Matches(text);
                if (matches.Count > 1 && text.Contains("раз") == false && text.Contains("рота") == false)
                {
                    return true;
                }

                foreach (Attachment attach in message.Attachments)
                {
                    if (attach.Type == typeof(Wall))
                    {
                        Wall wallPost = (Wall)attach.Instance;
                        text = wallPost.Text;
                        matches = regex.Matches(text);
                        if (matches.Count > 1 && text.Contains("раз") == false && text.Contains("рота") == false)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}
