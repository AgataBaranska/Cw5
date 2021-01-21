
using Cw5.DAL;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }


        [HttpGet("{index}")]
        public IActionResult GetStudent([FromRoute] string index)
        {
            var student = _dbService.GetStudent(index);
            if (student == null) return NotFound($"W bazie nie ma studenta o id: {index}");
            return Ok(student);
        }


        [HttpDelete("{index}")]
        public IActionResult deleteStudent([FromRoute] string index)
        {
            var rowsAffected = _dbService.RemoveStudent(index);
            if (rowsAffected == 0) return NotFound($"Studenta o podanym id: {index} nie ma w bazie");
            return Ok("Usuwanie ukończone");
        }

        [HttpPut("{index}")]
        public IActionResult updateStudent([FromRoute] string index, [FromBody] Student newStudent)
        {

            newStudent.IndexNumber = index;
            var rowsAffected = _dbService.UpdateStudent(newStudent);
            if (rowsAffected == 0) return NotFound($"Student o podanym {index} nie znajduje się w bazie");
            return Ok($"Aktualizacja dokończona");
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] Student student)
        {

            var rowsAffected = _dbService.AddStudent(student);

            if (rowsAffected == 0) return NotFound("Nie dodano studenta do bazy");
            return Ok("Dodano studenta do bazy");
        }

        [HttpGet("enroll/{index}")]//api/students/enroll/index

        public IActionResult GetStudentsEnrollment([FromRoute] string index)
        {


            var studentsEnrollment = _dbService.GetStudentsEnrollment(index);
            if (studentsEnrollment == null) return NotFound($"Nie odnaleziono zapisów studenta {index}");
            return Ok(studentsEnrollment);

        }


    }
}
