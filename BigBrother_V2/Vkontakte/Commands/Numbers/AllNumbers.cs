using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Numbers
{
    class AllNumbers : Command
    {
        public override string Name => "Номера начальства";

        MessagesSendParams @params = new MessagesSendParams();

        public override void Execute(Message message, VkApi client)
        {
            string answer;
            answer = new ChesnokNumber().Number + "\n\n";
            answer += new DekanNumber().Number + "\n\n";
            answer += new DekanatNumber().Number + "\n\n";
            answer += new DocumentovodNumber().Number + "\n\n";
            answer += new Electric_SantehnicNumber().Number + "\n\n";
            answer += new GolovanovNomber().Number + "\n\n";
            answer += new Kabinet8Number().Number + "\n\n";
            answer += new OperNumber().Number + "\n\n";
            answer += new SorokinaNumber().Number + "\n\n";
            answer += new UchebCentrElizarNumber().Number + "\n\n";
            answer += new Voen_CafedraNomber().Number + "\n\n";
            answer += new ZamDekNumber().Number + "\n";
            @params.PeerId = message.PeerId;
            @params.Message = answer;
            @params.RandomId = new Random().Next();
            Send(@params, client);
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("начальств"))
                return true;
            return false;
        }
    }
}
