using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Stucked.API.Controllers
{
    public class HighwaySignController : ApiController
    {
        // GET api/sign
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/sign/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/sign
        public void Post([FromBody]string value)
        {
        }

        // PUT api/sign/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sign/5
        public void Delete(int id)
        {
        }
    }
}
