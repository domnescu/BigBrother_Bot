using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    internal class UchebCentrElizarNumber : Command
    {
        public override string Name => "Номер учебного центра на Елизаре";

        public string Number = "Учебный центр на Елизаровской - 8(812)459-47-29";
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
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("учебн") && (text.Contains("центр") || text.Contains("елизар"));
        }
    }
}
