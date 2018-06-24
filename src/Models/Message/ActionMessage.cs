using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class ActionMessage : MessageBase
    {
        public MinutzAction Action { get; set; }
        public IEnumerable<MinutzAction> Actions { get; set; }
    }
}