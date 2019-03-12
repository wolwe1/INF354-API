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

        EmployeeDBEntity db = new EmployeeDBEntity();
        List<string> departments = new List<string>();
 
        public List<dynamic> sortDepartments(List<dynamic> emps)
        {
           departments.Clear();
            departments.Add("HQ");
            departments.Add("Logistics");
            departments.Add("Engineering");
            departments.Add("Operations");
            departments.Add("Un-assigned");

            List<dynamic> sortedEmployees = new List<dynamic>(); //Simply sorts by department for nicer display

            foreach (string dept in departments)
            {
                foreach(dynamic emp in emps)
                {
                    if (emp.department == dept)
                        sortedEmployees.Add(emp);
                }
            }
            return sortedEmployees;

        }
        [System.Web.Http.Route("api/Employee/getAll")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAll()
        {
            HttpResponseMessage response;
            try
            {
                
                List<Employee> employees = db.Employees.ToList();
                List<dynamic> temp = new List<dynamic>();
                List<dynamic> jsonReturn = new List<dynamic>();
                EmployeeExtra extra;

                foreach (Employee emp in employees)
                {
                    extra = db.EmployeeExtras.Where(x => x.id == emp.id).FirstOrDefault();

                    dynamic obj = new ExpandoObject();
                    obj.name = emp.name;
                    obj.surname = emp.surname;
                    obj.age = emp.age;
                    obj.department = "Un-assigned";

                    if (extra != null)
                        obj.department = extra.department;

                    temp.Add(obj); //for non http request mode
                }

                jsonReturn = sortDepartments(temp);

                response = Request.CreateResponse(HttpStatusCode.OK, jsonReturn);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                return response;    //Successful return */

            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                response = Request.CreateResponse(HttpStatusCode.OK, e.ToString());
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            return response;
        }

        [System.Web.Http.Route("api/Employee/add")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage add(string name,string surname,string age)
        {
            HttpResponseMessage failResponse = Request.CreateResponse(HttpStatusCode.OK, false);
            failResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            HttpResponseMessage successResponse = Request.CreateResponse(HttpStatusCode.OK, true);
            successResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            if (name == "" || surname == "" || age == "")
                return failResponse;

            Employee lastEmp = db.Employees.ToList().Last();

            Employee newEmployee = new Employee();

            newEmployee.name = name;
            newEmployee.surname = surname;
            newEmployee.age = age;
            newEmployee.id = lastEmp.id + 1;

            db.Employees.Add(newEmployee);
            db.SaveChanges();
            return successResponse;

        }

        public HttpResponseMessage getEmployee(string name, string surname)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, false);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            HttpResponseMessage failureResponse;
            HttpResponseMessage success;

            dynamic employee = new ExpandoObject();

            Employee emp = db.Employees.Where(x => x.name == name && x.surname == surname).FirstOrDefault();

            if(emp == null)
            {
                failureResponse = Request.CreateResponse(HttpStatusCode.OK, "NO EMPLOYEE MATCHES THIS DESCRIPTION");
                failureResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            }else
            {
                EmployeeExtra extra = db.EmployeeExtras.Where(xx => xx.id == emp.id).FirstOrDefault();

                if(extra == null)
                {
                    failureResponse = Request.CreateResponse(HttpStatusCode.OK, "NO EMPLOYEE MATCHES THIS DESCRIPTION");
                    failureResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                }else
                {
                    employee.name = emp.name;
                    employee.surname = emp.surname;
                    employee.age = emp.age;
                    employee.department = extra.department;
                    employee.title = extra.title;
                    employee.salary = extra.salary;

                    string ret = JsonConvert.SerializeObject(employee);
                    success = Request.CreateResponse(HttpStatusCode.OK,ret);
                    success.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            }
            return response;

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