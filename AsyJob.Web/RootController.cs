using Microsoft.AspNetCore.Mvc;

namespace AsyJob.Web
{
    [Route("api")]
    [ApiController]
    public class RootController
    {
        [HttpGet]
        public Sitemap GetLinks()
        {
            return new Sitemap();
        }
    }
}
