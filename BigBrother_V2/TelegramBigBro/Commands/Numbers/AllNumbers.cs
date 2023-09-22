using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    internal class AllNumbersTelegram : CommandTelegram
    {
        public override string Name => "Номера начальства";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            string answer;
            answer = new ChesnokNumberTelegram().Number + "\n\n";
            answer += new ChesnokMailTelegram().Number + "\n\n";
            answer += new DekanNumberTelegram().Number + "\n\n";
            answer += new DekanatNumberTelegram().Number + "\n\n";
            answer += new DocumentovodNumberTelegram().Number + "\n\n";
            answer += new Electric_SantehnicNumberTelegram().Number + "\n\n";
            answer += new GolovanovNomberTelegram().Number + "\n\n";
            answer += new Kabinet8NumberTelegram().Number + "\n\n";
            answer += new OperNumberTelegram().Number + "\n\n";
            answer += new SorokinaNumberTelegram().Number + "\n\n";
            answer += new UchebCentrElizarNumberTelegram().Number + "\n\n";
            answer += new Voen_CafedraNomberTelegram().Number + "\n\n";
            answer += new ZamDekNumberTelegram().Number + "\n\n";
            answer += new MedChastiTelegram().Number + "\n\n";
            answer += new AccountingTelegram().Number + "\n\n";
            answer += new PasspornNumberTelegram().Number + "\n";
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: answer,
                cancellationToken: cancellationToken
            );

        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("начальств");
        }
    }
}
