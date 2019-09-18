using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apiSixCore.Models;
using System.Web.Http.Cors;
using System.Web;

namespace apiSixCore.Controllers
{

    //cors
    [EnableCors(origins: "*", headers: "*", methods: "*")] 

    public class EmployeesController : ApiController
    {
        //Create instance of Linq-To-Sql class as db  
        EmployeesDataClassesDataContext db = new EmployeesDataClassesDataContext();


        //This action method returns all employee records.  
        // GET api/<controller> 
       // [Route("api/getAllemployees")]
        public  IEnumerable<Employee> Get()
        {
            //returning all records of table Employee.  
            return db.Employees.ToList().AsEnumerable();
        }

        //This action method will fetch and filter for specific Employee id record  
        // GET api/get<controller>/5  
        public HttpResponseMessage Get(string id)
        {
            //fetching and filter specific Employee id record   
            var employeedetails = (from e in db.Employees where e.EmployeeNumber == id select e).FirstOrDefault();

           
            //checking fetched or not with the help of NULL or NOT.  
            if (employeedetails != null)
            {

                ////calculatng age
                int empyear = Convert.ToInt32(employeedetails.DateOfBirth.Year);
                int empdate = Convert.ToInt32(employeedetails.DateOfBirth.DayOfYear);
                int age = 0;
                age = DateTime.Now.Year - empyear;
                if (DateTime.Now.DayOfYear < empdate)
                    age = age - 1;

               //sending response as status code OK with employeetdetails and age entity.  
                return Request.CreateResponse(HttpStatusCode.OK, new { employeedetails,age});
            }
            else
            {
                //sending response as error status code NOT FOUND with meaningful message.  
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Employee Not Found");
            }
        }


        //To add a new Employee record  
        // POST api/add<controller>  
        public HttpResponseMessage Post([FromBody]Employee _employee)
        {
            try
            {
                //To add an new employee record  
                db.Employees.InsertOnSubmit(_employee);

                //Save the submitted record  
                db.SubmitChanges();

                //return response status as successfully created with employee entity  
                var msg = Request.CreateResponse(HttpStatusCode.Created, _employee);

                //Response message with requesturi for check purpose  
                msg.Headers.Location = new Uri(Request.RequestUri + _employee.EmployeeNumber);

                return msg;
            }
            catch (Exception ex)
            {

                //return response as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //To update employww record  
        // PUT api/update<controller>/5  
        public HttpResponseMessage Patch(string id, [FromBody]Employee employee)
        {
            //fetching and filter specific employee id record   
            var employeedetails = (from e in db.Employees where e.EmployeeNumber == id select e).FirstOrDefault();

            //checking fetched or not with the help of NULL or NOT.  
            if (employeedetails != null)
            {
                //set received _employee object properties with employeedetail  
                employeedetails.EmployeeNumber = employee.EmployeeNumber;
                employeedetails.DateOfBirth = employee.DateOfBirth;
                employeedetails.Name = employee.Name;
                //save set allocation.  
                db.SubmitChanges();

                //return response status as successfully updated with employee entity  
                return Request.CreateResponse(HttpStatusCode.OK, employeedetails);
            }
            else
            {
                //return response error as NOT FOUND  with message.  
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Employee Not Found");
            }


        }

         // DELETE api/delete<controller>/5  
        public HttpResponseMessage Delete(string id)
        {

            try
            {
                //fetching and filter specific employee id record   
                var _deleteEmployee = (from e in db.Employees where e.EmployeeNumber == id select e).FirstOrDefault();

                //checking fetched or not with the help of NULL or NOT.  
                if (_deleteEmployee != null)
                {

                    db.Employees.DeleteOnSubmit(_deleteEmployee);
                    db.SubmitChanges();

                    //return response status as successfully deleted with employee id  
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    //return response error as Not Found  with exception message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee Not Found or Invalid " + id.ToString());
                }
            }

            catch (Exception ex)
            {

                //return response error as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


    




    }
}