using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.TestHarness
{
    /// <summary>
    /// Responsible for running a SetUp task before an integration test is run
    /// <br/>
    /// Any non-nullable properties may only be initialized after <see cref="SetUp"/> is run. To avoid <see cref="NullReferenceException"/>
    /// only access properties inside your test
    /// </summary>
    internal interface ISetUp
    {
        Task SetUp();
    }
}
