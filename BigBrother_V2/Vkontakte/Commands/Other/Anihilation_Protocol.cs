using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Other
{
    /// <summary>
    /// Данная функция не входит в релизную версию, она используется исключительно для тестирования функций
    /// </summary>
    class Anihilation_Protocol : Command
    {
        public override string Name => "Протокол уничтожения беседы";

        MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            @params.Message = "[id449214904|Чай] тут Главный следователь - она уже давно ищет вибратор\n Я бы сказал что она хреновый следователь))";
            @params.DisableMentions = true;
            @params.PeerId = message.PeerId.Value;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("протокол") && text.Contains("анигиляция"))
            {
                return true;
            }

            return false;
        }
    }
}