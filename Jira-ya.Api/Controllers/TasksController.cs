using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Jira_ya.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _taskService.GetAll();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = _taskService.GetById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskDto dto)
        {
            var created = _taskService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TaskDto dto)
        {
            var updated = _taskService.Update(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _taskService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
