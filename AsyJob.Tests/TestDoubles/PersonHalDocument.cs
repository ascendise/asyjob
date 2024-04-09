using AsyJob.Web.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Web.Tests.TestDoubles
{
    internal class PersonHalDocument(string name, int age) : HalDocument
    {
        public string Name { get; set; } = name;
        public int Age { get; set; } = age;

    }
}
