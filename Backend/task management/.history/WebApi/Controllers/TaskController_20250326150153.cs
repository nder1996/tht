using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskRequest taskRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                    
                return BadRequest(ResponseApiBuilderService.Failure<object>(
                    errorCode: "VALIDATION_ERROR",
                    errorDescription: string.Join(", ", errors),
                    statusCode: 400
                ));
            }

            var response = await _taskService.CreateTask(taskRequest);
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }
    }
} 