﻿using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands
{
    class Kabinet8Number : Command
    {
        public override string Name => "Номер 8 кабинета";

        public string Number = "8 кабинет - 8(911)721-19-04";

        MessagesSendParams @params = new MessagesSendParams();

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && ((text.Contains("8") || text.Contains("восьм")) && text.Contains("кабинет")))
                return true;
            return false;
        }
    }
}
