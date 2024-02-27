
using AddressBookWebApp.Context;
using AddressBookWebApp.Models;
using AddressBookWebApp.Controllers.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AddressBookWebApp.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly AddressBookContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<AppUser> userManager,
                                 AddressBookContext context,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            AppUser user = new AppUser{ UserName = model.LoginProp };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            return BadRequest();
            }
        }




        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(EntryViewModel model)
        {
            AppUser targetUser = null;
            var userList = _context.Users.Where(x => x.UserName == model.LoginProp);
            foreach(var user in userList) 
            {
                PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    targetUser = user;
                    break;
                }
            }
            
            if(targetUser != null) 
            {
                var role = from ur in _context.UserRoles
                           join r in _context.Roles on ur.RoleId equals r.Id
                           where ur.UserId == targetUser.Id
                           select new UserRole { Role = r.Name };
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
                var symmetricSecurityKey = new SymmetricSecurityKey(signingKey);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, targetUser.UserName),
                                                        new Claim(ClaimTypes.Role, role.First<UserRole>().Role),
                                                        new Claim(ClaimTypes.NameIdentifier,targetUser.Id)}),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(securityToken);

                return Ok(new
                {
                    UserName = targetUser.UserName,
                    UserRole = role.First<UserRole>().Role,
                    Token = tokenString
                });

            }
            return NotFound();

        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserList()
        {
            var users = from user in _context.Users
                        join userRole in _context.UserRoles on user.Id equals userRole.UserId
                        join roles in _context.Roles on userRole.RoleId equals roles.Id
                        select new UserModelForAdmin
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            UserRole = roles.Name,
                        };

            return Ok(users);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser.Id != id)
            {

                var targetUser = _context.Users.SingleOrDefault(x => x.Id == id);
                if (targetUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(targetUser);
                    await _userManager.RemoveFromRolesAsync(targetUser, roles);
                    await _userManager.DeleteAsync(targetUser);
                    return Ok();
                }
                else return NotFound();
            }
            else return BadRequest("Вы пытаетесь удалить себя");

        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUserRole(string id, UserRole role)
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser.Id != id)
            {
                AppUser targetUser = _context.Users.SingleOrDefault(x => x.Id == id);
                var roles = await _userManager.GetRolesAsync(targetUser);
                await _userManager.RemoveFromRolesAsync(targetUser,roles);
                string newrole = "user";
                if (string.Compare(role.Role, "user") == 0) newrole = "admin";
                if (string.Compare(role.Role, "admin") == 0) newrole = "user";
                await _userManager.AddToRoleAsync(targetUser, newrole);
                return Ok();
            }
            else return BadRequest("Вы пытаетесь изменить роль администратора на роль пользователя");
          
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(RegisterViewModelForAdmin model)
        {
            AppUser user = new AppUser{ UserName = model.LoginProp };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.UserRole);
                return Ok();
            }
            else return BadRequest();
            
        }

    }

}
