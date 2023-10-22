using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    internal class GolovanovNomber : Command
    {
        public override string Name => "Номер Голованова";

        public string Number = "Начальник учебной части - заместитель начальника военного учебного центра Головатов Сергей Анатольевич 8(812)321-15-92";
        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId;
            @params.Message = Number;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("голованов");
        }
    }
}