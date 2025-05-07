using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlineSalesSystem.Core.Services;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Api.DTOs;
using OnlineSalesSystem.API.DTOs;
using Microsoft.AspNetCore.Identity.Data;


[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost("login")]
   public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            if (token == null)
                return Unauthorized("Credenciais inválidas");

            return Ok(new { token });
        }

        
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.RegisterUser(registerDTO.Username, registerDTO.Role, registerDTO.Password);

        return Ok(new { message = "Usuário registrado com sucesso!" });
    }

        public record LoginRequest(string Username, string Password);
}