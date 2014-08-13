using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stucked.Model;
using Stucked.DataAccess;

namespace Stucked.API.Controllers
{
    public class HighwayController : ApiController
    {
        private StuckedContext db = new StuckedContext();

        // GET api/sign
        public IEnumerable<Highway> Get()
        {
            return db.Highways.ToList();
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
