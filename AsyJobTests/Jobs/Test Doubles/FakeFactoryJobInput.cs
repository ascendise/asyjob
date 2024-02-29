using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class FakeFactoryJobInput(int checknum)
    {
        public int CheckNum { get; } = checknum;
    }
}
