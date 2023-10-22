using System;
using VkNet;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    internal class DocumentovodNumber : Command
    {
        public override string Name => "Номер ведущих документоведов";

        public string Number = "Ведущие документоведы: Жукова Юлия Евгеньевна и Власенкова Светлана Николаевна - 8(812)321 - 15 - 92";
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
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("документ");
        }
    }
}