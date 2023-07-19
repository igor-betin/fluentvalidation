using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidationSlim.Examples.Domain
{
    internal class TouristGroupRequest
    {
        public List<Person> Tourists { get; set; }

        public Person Guide { get; set; }
    }
}
