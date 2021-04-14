using BlogLab.Models.Account;
using BlogLab.Service;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly SignInManager<ApplicationUserIdentity> _signInManager;

        public AccountController(ITokenService tokenService, UserManager<ApplicationUserIdentity> userManager, SignInManager<ApplicationUserIdentity> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register(ApplicationUserCreate applicationUserCreate)
        {
            var applicationUserIdentity = applicationUserCreate.Adapt<ApplicationUserIdentity>();

            var result = await _userManager.CreateAsync(applicationUserIdentity, applicationUserCreate.Password);

            if (result.Succeeded)
            {
                var applicationUser = applicationUserIdentity.Adapt<ApplicationUser>();

                applicationUser.Token = _tokenService.CreateToken(applicationUserIdentity);

                return applicationUser;
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> Login(ApplicationUserLogin applicationUserLogin)
        {
            var applicatonUserIdentity = await _userManager.FindByNameAsync(applicationUserLogin.Username);

            if (applicationUserLogin != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(
                    applicatonUserIdentity, applicationUserLogin.Password, false);
                if (result.Succeeded)
                {
                    ApplicationUser applicationUser = applicatonUserIdentity.Adapt<ApplicationUser>();

                    applicationUser.Token = _tokenService.CreateToken(applicatonUserIdentity);

                    return Ok(applicationUser);
                }
            }

            return BadRequest("Invalid login attempt.");
        }
    }
}