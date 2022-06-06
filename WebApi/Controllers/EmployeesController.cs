using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Contract.Response;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private ApplicationDbContext dbContext;
        private ApiResponse response = new ApiResponse();
        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetEmployee")]
        public IActionResult GetEmployee()
        {
            var emp = dbContext.employees.ToList();
            response.Ok = true;
            response.Message = "Employee list found !";
            response.Status = 200;
            response.Data = emp;
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateEmployee")]
        public IActionResult CreateEmployee(Employee e)
        {
            try
            {
                dbContext.employees.Add(e);
                dbContext.SaveChanges();

                response.Ok = true;
                response.Message = "create successfully";
                response.Status = 200;
                response.Data = e;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message =ex.Message;
                response.Status = 500;
                response.Ok = false;
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateEmp")]
        public IActionResult UpdateEmp(Employee emp)
        {
            try
            {
                var up = dbContext.employees.SingleOrDefault(e => e.Id == emp.Id);
                if (up != null)
                {
                    up.Name = emp.Name;
                    up.Mobile = emp.Mobile;
                    dbContext.employees.Update(up);
                    dbContext.SaveChanges();
                    response.Ok = true;
                    response.Message = "Update successfully";
                    response.Status = 200;
                    response.Data = up;
                }
                else
                {
                    response.Ok = false;
                    response.Message = "Employee not found";
                    response.Status = 404;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = 500;
                response.Ok = false;
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteEmp/{id}")]
        public IActionResult DeleteEmp(int id)
        {
            var dl = dbContext.employees.SingleOrDefault(e => e.Id == id);
            if(dl != null)
            {
                dbContext.employees.Remove(dl);
                dbContext.SaveChanges();
                response.Ok = true;
                response.Status = 200;
                response.Message = "Delete successfully";
            }
            else
            {
                response.Ok = false;
                response.Status = 404;
                response.Message = "Employee not found";
            }
            return Ok(response);
        }
    }
}
  