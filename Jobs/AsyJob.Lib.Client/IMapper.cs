using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Client
{
    internal interface IMapper<TSrc, TDest>
    {
        TDest Map(TSrc src);
    }
}
