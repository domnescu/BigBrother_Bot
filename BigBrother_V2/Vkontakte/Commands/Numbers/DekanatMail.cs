using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;


namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    class DekanatMail:Command
    {
        public override string Name => "Почта Деканата";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            if (message.FromId.Value == 143676891)
                @params.Message = "Да ты уже заебал! Я уже заебался отправлять тебе почту деканата";
            else
                @params.Message = "Почта деканата - dekanatoif@mail.ru";
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("почта") || text.Contains("у кого")) && text.Contains("почта") && text.Contains("деканат"))
                return true;
            return false;
        }
    }
}

