using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class InstanceResponse : MessageBase
    {
        public Instance Instance { get; set; }
        public List<Instance> Instances { get; set; }
    }
}