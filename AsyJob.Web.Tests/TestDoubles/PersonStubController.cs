using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Web.Tests.TestDoubles
{
    [ApiController]
    [Route("/api/person/")]
    internal class PersonStubController : ControllerBase
    {
        [HttpGet("/withid/{id}")]
        public static PersonHalDocument Get(Guid _)
        {
            return null!;
        }
    }
}
