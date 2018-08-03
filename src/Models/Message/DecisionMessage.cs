using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class DecisionMessage : MessageBase
    {
        public List<MinutzDecision> DecisionCollection { get; set; }
        public MinutzDecision Decision { get; set; }
    }
}