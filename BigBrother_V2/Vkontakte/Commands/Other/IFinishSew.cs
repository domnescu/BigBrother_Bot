using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    internal class IFinishSew : Command
    {
        public override string Name => "Пустая Команда";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            User user = new(message.PeerId.Value, client);
            bool Succes = db.AddToDB("DELETE FROM WhoSew WHERE domain='[id" + user.Id + "|" + user.FirstName + " " + user.LastName + "]';");

            @params.Message = Succes ? "Хорошо, я запомнил что ты больше не шьёшь." : "Так тебя и небыло в списке людей которые умеют шить";

            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return ((text.Contains("могу") && text.Contains(" шить")) || text.Contains("шью")) && text.Contains("не") && message.PeerId.Value < 2000000000;
        }
    }
}
