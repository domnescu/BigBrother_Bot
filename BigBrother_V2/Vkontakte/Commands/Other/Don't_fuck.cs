using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class Dont_fuck : Command
    {

        public override string Name => "Шуточный ответ на \"не ебёт\"";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            User user = new(message.FromId.Value, client);
            if (user.Sex == VkNet.Enums.Sex.Male)
            {
                @params.Message = "А схуяли тебя что-то или кто-то должен ебать ?";
            }
            else if (user.Sex == VkNet.Enums.Sex.Female)
            {
                @params.Message = "Эт легко исправить! @all Парни!! тут " + user.FirstName + " хочет чтоб её ебали!";
            }
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("не ебёт") || text.Contains("не ебет");
        }
    }
}
