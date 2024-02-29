using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class FakeGuidProvider : IGuidProvider
    {
        public Guid[] Guids { get; set; }
        private int _lastIndex = -1;

        public FakeGuidProvider(Guid[] guids)
        {
            if(guids.Length == 0)
            {
                throw new ArgumentException($"{nameof(guids)} cannot be empty");
            }
            Guids = guids;
        }

        public Guid GetGuid()
        {
            _lastIndex++;
            if(_lastIndex >= Guids.Length)
            {
                _lastIndex = 0;
            }
            return Guids[_lastIndex];
        }
    }
}
