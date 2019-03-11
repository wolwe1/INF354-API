using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INF354_API.Models;
using System.Diagnostics;
using System.Dynamic;
using Newtonsoft.Json;

namespace INF354_API.Controllers
{
    public class EmployeeController : ApiController
    {

        EmployeeDBEntities db = new EmployeeDBEntities();

        // GET api/<controller>
        [System.Web.Http.Route("api/Employee/getAll")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage getAll()
        {
            HttpResponseMessage response;
            try
            {

                //Method 1 Using httpResponse
                List<Employee> employees = db.Employees.ToList();
                List<dynamic> jsonReturn = new List<dynamic>();
                List<string> JSON = new List<string>();

                foreach (Employee emp in employees)
                {
                    dynamic obj = new ExpandoObject();
                    obj.name = emp.name;
                    obj.surname = emp.surname;
                    obj.age = emp.age;
                    string json = JsonConvert.SerializeObject(obj);
                    JSON.Add(json);
                    jsonReturn.Add(obj); //for non http request mode
                }

                //return jsonReturn;
                //string json = JsonConvert.SerializeObject(jsonReturn);
                 
                response = Request.CreateResponse(HttpStatusCode.OK,JSON);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                return response;    //Successful return */

            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            /* response = new HttpResponseMessage(); //Empty return
             return response; */
            return null;
        }


        [System.Web.Http.Route("api/Employee/add")]
        [System.Web.Http.HttpPost]
        public bool add(string name,string surname,string age)
        {
            if (name == "" || surname == "" || age == "")
                return false;

            Employee lastEmp = db.Employees.ToList().Last();

            Employee newEmployee = new Employee();

            newEmployee.name = name;
            newEmployee.surname = surname;
            newEmployee.age = age;
            newEmployee.id = lastEmp.id + 1;

            db.Employees.Add(newEmployee);
            db.SaveChanges();
            return true;

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