using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    /// <summary>
    /// Responsible for running a SetUp task before an integration test is run
    /// </summary>
    internal interface ISetUp
    {
        Task SetUp();
    }
}
