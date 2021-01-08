
using Cw5.DTOs.Requests;
using Cw5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IStudentsDbService _dbService;

        public EnrollmentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;
        }


        [HttpPost]
        public IActionResult StartStudentsEnroll([FromBody] EnrollStudentRequest request)
        {
            try
            {
                var enrollmentResult = _dbService.StartEnrollStudent(request);
                if (enrollmentResult == null) return BadRequest("EnrollmentResult is null");
                return  Created($"api/students/enrollments", enrollmentResult);


            }
            catch (EnrollmentException e)
            {

                return BadRequest(e.Message);
            }

           
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents([FromBody]PromoteStudentsRequest request)
        {
            var promotionResult = _dbService.PromoteStudents(request);
            if (promotionResult == null)
            {
                return BadRequest($"W bazie danych nie istnieje wpis dotyczący {request.Studies} i semsttru {request.Semester}");

            }

            return Created("api/students", promotionResult);

        }


    }
}
