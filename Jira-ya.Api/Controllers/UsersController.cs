using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jira_ya.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest dto)
        {
            var result = await _userService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateUserRequest dto)
        {
            var result = await _userService.UpdateAsync(id, dto);
            if (!result.Success)
            {
                if (result.Error == "Usuário não encontrado.")
                    return NotFound();
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Success)
            {
                if (result.Error == "Usuário não encontrado.")
                    return NotFound();
                return BadRequest(result.Error);
            }
            return NoContent();
        }

        [HttpPost("createRandom")]
        public async Task<IActionResult> CreateRandom([FromBody] CreateRandomUsersRequest request, [FromServices] IConfiguration config)
        {
            var randomKey = config["RandomUserKey"] ?? "RND";
            var result = await _userService.CreateRandomUsersAsync(request.Amount, request.UserNameMask, randomKey);
            if (!result.Success)
                return BadRequest(result.Error);
            return Ok(result.Data);
        }
    }
}
