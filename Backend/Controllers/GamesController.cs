using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// Defines routes and handles actions related to games.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        public IActionResult Example()
        {
            var testObject = new
            {
                Id = 8,
                Text = "Hello"
            };
            return Ok(testObject);
        }
    }
}
