using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        string[] studentNames = new string[] { "John", "Jane", "Mark", "Emily", "David"};

        return Ok(studentNames);
    }
}