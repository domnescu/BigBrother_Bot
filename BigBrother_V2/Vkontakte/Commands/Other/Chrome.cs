using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    class Chrome : Command
    {

        public override string Name => "Главный бизнесмен Макары";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "https://www.instagram.com/chrome_krh/ Расскажет вам все секреты ведения бизнеса, в частности про то, как инвестировать деньги " +
                "и не получить нихрена взамен.";
            @params.DisableMentions = true;
            @params.DontParseLinks = true;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("бизнес"))
                return true;
            return false;
        }
    }
}
