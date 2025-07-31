using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jira_ya.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ITaskService taskService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var tasks = await taskService.GetByUserIdAsync(userId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest dto)
        {
            var result = await taskService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskDto dto)
        {
            var result = await taskService.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await taskService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            return NoContent();
        }

        [HttpPost("{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignTask(Guid taskId, Guid userId)
        {
            var result = await taskService.AssignTaskAsync(taskId, userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
