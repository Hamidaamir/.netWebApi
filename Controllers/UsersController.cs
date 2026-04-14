using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user is null)
                return Problem(
                    title: "User not found",
                    detail: $"No user with id {id} exists.",
                    statusCode: 404);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UserDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result is null)
                return Problem(
                    title: "User not found",
                    detail: $"No user with id {id} exists.",
                    statusCode: 404);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return Problem(
                    title: "User not found",
                    detail: $"No user with id {id} exists.",
                    statusCode: 404);

            return NoContent();
        }
    }
}
