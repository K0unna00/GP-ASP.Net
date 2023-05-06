using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tester.Data;
using tester.Models;

namespace tester.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public JsonResult Login()
        {
            return new JsonResult(true);
        }
        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginModel user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
            if (result.Succeeded)
            {
                return new JsonResult(true);
            }
            return new JsonResult(false);

            
        }
        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterModel user)
        {
            IdentityUser newUser = new IdentityUser()
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                _context.SaveChanges();

                return new JsonResult(true);
            }
            else
            {
                return new JsonResult(false);
            }
        }
    }
}
