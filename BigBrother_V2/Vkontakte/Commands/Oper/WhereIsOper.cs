using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte.Commands.Oper
{
    internal class WhereIsOper : Command
    {
        public override string Name => "Где опер?";

        private readonly MessagesSendParams @params = new();

        public override void Execute(Message message, VkApi client)
        {
            Database db = new();
            string Location;
            Dictionary<string, string> WarningList = db.GetDictionaryString("WarningList");
            foreach (KeyValuePair<string, string> warning in WarningList)
            {
                if (message.Text.ToLower().Contains(warning.Key.ToLower()) || message.Text.ToLower().StartsWith("ень"))
                {
                    Location = db.GetString("WarningList", "Type", warning.Value, 2);
                    @params.Message = Location;
                    @params.PeerId = message.PeerId.Value;
                    @params.RandomId = new Random().Next();
                    Send(@params, client);
                    break;
                }
            }
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("где") && (db.CheckText(text, "WarningList") || text.StartsWith("ень"));
        }
    }
}
