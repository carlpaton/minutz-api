using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class PersonResponse : MessageBase
    {
       public Person Person { get; set; }
       public IEnumerable<Person> People { get; set; }
    }
}