using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        // GET: /api/Student
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }

        // GET: /api/Student/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _service.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // POST: /api/Student
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var createdStudent = await _service.CreateAsync(student);

            return Ok(createdStudent);
        }

        // PUT: /api/Student/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            var result = await _service.UpdateAsync(id, student);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Student updated successfully");
        }

        // DELETE: /api/Student/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Student deleted successfully");
        }
    }
}