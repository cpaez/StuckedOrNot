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
    public class HighwaySignController : ApiController
    {
        private StuckedContext db = new StuckedContext();

        // GET api/highwaysign
        public IEnumerable<HighwaySign> Get()
        {
            return db.HigwaySigns.ToList();
        }

        // GET api/highwaysign/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/highwaysign
        public void Post([FromBody]string value)
        {
        }

        // PUT api/highwaysign/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/highwaysign/5
        public void Delete(int id)
        {
        }
    }
}
