using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text;
using LegionCore.Core.Identity;
using LegionCore.Core.Models.Api;
using LegionCore.Core.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

// * [FromXXX] is required for ApiControllers. [FromBody] is default, and is used for the Body. [FromForm] is used for the Form, and what we want.
namespace LegionCore.Web
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Area("Identity")]
    public class ApiAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ApiAuthController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<string> Test()
        {
            return "It works!!";
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ApiLoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var ApplicationRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in ApplicationRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApiRegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(HttpStatusCode.BadRequest, null, "User already exists!"));

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(HttpStatusCode.BadRequest,
                        "User creation failed! Please check user details and try again."));

            return Ok(new ApiResponse(HttpStatusCode.OK, "User created successfully!"));
        }

        // [HttpPost]
        // public async Task<IActionResult> RegisterAdmin([FromBody] ApiRegisterModel model)
        // {
        //     var userExists = await _userManager.FindByNameAsync(model.Username);
        //     if (userExists != null)
        //         return StatusCode(StatusCodes.Status500InternalServerError,
        //             new ApiResponse(HttpStatusCode.BadRequest, null, "User already exists!"));
        //
        //     ApplicationUser user = new()
        //     {
        //         Email = model.Email,
        //         SecurityStamp = Guid.NewGuid().ToString(),
        //         UserName = model.Username
        //     };
        //     var result = await _userManager.CreateAsync(user, model.Password);
        //     if (!result.Succeeded)
        //         return StatusCode(StatusCodes.Status500InternalServerError,
        //             new ApiResponse(HttpStatusCode.BadRequest, null,
        //                 "User creation failed! Please check user details and try again."));
        //
        //     if (!await _roleManager.RoleExistsAsync(ApplicationRole.Admin))
        //         await _roleManager.CreateAsync(new ApplicationRole(ApplicationRole.Admin));
        //     if (!await _roleManager.RoleExistsAsync(ApplicationRole.User))
        //         await _roleManager.CreateAsync(new ApplicationRole(ApplicationRole.User));
        //
        //     if (await _roleManager.RoleExistsAsync(ApplicationRole.Admin))
        //     {
        //         await _userManager.AddToRoleAsync(user, ApplicationRole.Admin);
        //     }
        //
        //     if (await _roleManager.RoleExistsAsync(ApplicationRole.Admin))
        //     {
        //         await _userManager.AddToRoleAsync(user, ApplicationRole.User);
        //     }
        //
        //     return Ok(new ApiResponse(HttpStatusCode.OK, null, "User created successfully!"));
        // }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}