using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apiSixCore.Models;
using System.Web.Http.Cors;


namespace apiSix.Controllers
{
    //cors
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class DepartmentsController : ApiController

    {

        //Create instance of Linq-To-Sql class as db  
        DepartmentsDataClassesDataContext db = new DepartmentsDataClassesDataContext();

        //This action method return all departments records.  
        // GET api/<controller>  
        public IEnumerable<Department> Get()
        {
            //returning all records of table Departments.  
            return db.Departments.ToList().AsEnumerable();
        }

           //This action method will fetch and filter for specific department id record  
        // GET api/get<controller>/5  
        public HttpResponseMessage get(int id)
        {
            //fetching and filter specific department id record   
            var departmentDetail = (from a in db.Departments where a.Id == id select a).FirstOrDefault();


            //checking fetched or not with the help of NULL or NOT.  
            if (departmentDetail != null)
            {
                //sending response as status code OK with departmentdetail entity.  
                return Request.CreateResponse(HttpStatusCode.OK, departmentDetail );
            }
            else
            {
                //sending response as error status code NOT FOUND with meaningful message.  
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Department Not Found");
            }
        }





        //To add a new Department record  
        // POST api/add<controller>  
        public HttpResponseMessage Post([FromBody]Department _department)
        {
            try
            {
                //To add an new department record  
                db.Departments.InsertOnSubmit(_department);

                //Save the submitted record  
                db.SubmitChanges();

                //return response status as successfully created with department entity  
                var msg = Request.CreateResponse(HttpStatusCode.Created, _department);

                //Response message with requesturi for check purpose  
                msg.Headers.Location = new Uri(Request.RequestUri + _department.Id.ToString());

                return msg;
            }
            catch (Exception ex)
            {

                //return response as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



        //To update department record  
        // PUT api/update<controller>/5  
        public HttpResponseMessage Put(int id, [FromBody]Department _department)
        {
            //fetching and filter specific department id record   
            var departmentdetail = (from a in db.Departments where a.Id == id select a).FirstOrDefault();

            //checking fetched or not with the help of NULL or NOT.  
            if (departmentdetail != null)
            {
                //set received _department object properties with departmentdetail  
                departmentdetail.Id = _department.Id;
                departmentdetail.Description = _department.Description;
                //save set allocation.  
                db.SubmitChanges();

                //return response status as successfully updated with department entity  
                return Request.CreateResponse(HttpStatusCode.OK, departmentdetail);
            }
            else
            {
                //return response error as NOT FOUND  with message.  
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or DepartMent Not Found");
            }


        }


        // DELETE api/delete<controller>/5  
        public HttpResponseMessage Delete(int id)
        {

            try
            {
                //fetching and filter specific department id record   
                var _deleteDepartment = (from a in db.Departments where a.Id == id select a).FirstOrDefault();

                //checking fetched or not with the help of NULL or NOT.  
                if (_deleteDepartment != null)
                {

                    db.Departments.DeleteOnSubmit(_deleteDepartment);
                    db.SubmitChanges();

                    //return response status as successfully deleted with department id  
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    //return response error as Not Found  with exception message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department Not Found or Invalid " + id.ToString());
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