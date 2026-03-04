using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // Todas as rotas exigem autenticação
    public class ProfileController : ControllerBase
    {

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            // Extrai o perfil completo do JWT
            var currentUser = JwtHelper.GetCurrentUser(User);

            if (currentUser == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Data = null,
                    Message = "Token inválido ou usuário não autenticado"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Data = currentUser,
                Message = "User data retrieved successfully"
            });
        }

        [AllowAnonymous]
        [HttpGet("public-info")]
        public IActionResult GetPublicInfo()
        {
            return Ok(new { message = "Esta rota é pública e não requer autenticação" });
        }
    }
}
