﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace INF354_API.Controllers
{
    public class EmployeeController : ApiController
    {
        // GET api/<controller>
        [System.Web.Http.Route("api/Employee/getAll")]
        [System.Web.Http.HttpPost]
        public IEnumerable<string> getAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}