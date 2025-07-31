using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Jira_ya.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest dto)
        {
            var created = _userService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateUserRequest dto)
        {
            var updated = _userService.Update(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
