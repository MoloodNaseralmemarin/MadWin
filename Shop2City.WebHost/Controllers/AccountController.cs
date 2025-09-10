using MadWin.Application.DTOs.Account;
using MadWin.Application.Services;
using MadWin.Application.ViewModels.Account;
using MadWin.Core.Common;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shop2City.Core.Convertors;
using Shop2City.WebHost.ViewModels.Account;
using System.Security.Claims;


namespace Shop2City.WebHost.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AccountController> _logger;
        private readonly ISmsSenderService _smsSenderService;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(IUserService userService, ILogger<AccountController> logger, ISmsSenderService smsSenderService, IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
            _smsSenderService = smsSenderService;
            _passwordHasher = passwordHasher;


        }
        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (await _userService.IsExistCellPhoneAsync(model.CellPhone))
            {
                ModelState.AddModelError("CellPhone", ErrorMessage.InvalidCellPhone);
                return View(model);
            }

            #region Add User
            model.Password = _passwordHasher.HashPassword(model.Password);
            var user = new User
            {
                Address = model.Address,
                CellPhone = model.CellPhone,
                FirstName=model.FirstName,
                LastName=model.LastName,
                Password=model.Password,
                UserName=model.CellPhone

            };
            await _userService.CreateUser(user);
            #endregion
            #region welcome SMS

            var smsSent = await _smsSenderService.SendWelcomeSmsAsync(model.CellPhone, model.FullName, 2);

            if (smsSent)
            {
                // فقط لاگ کن، بدون اطلاع دادن به کاربر
                _logger.LogWarning("ارسال پیامک خوش‌آمدگویی برای {Phone} موفق نبود. احتمالاً API پیامک خطا داده.", model.CellPhone);
            }

            #endregion
            return RedirectToAction("Login");
        }


   

        #endregion
        #region Login
        [HttpGet]
        [Route("Login")]
        public IActionResult Login(bool editProfile = false)
            {
                ViewBag.EditProfile = editProfile;
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState is invalid during login for {User}", model.userName);
                    return View(model);
                }
               // try
                //{
                    var result = await _userService.LoginAsync(new LoginResultDto
                    {
                        UserName = model.userName,
                        Password = model.password
                    });

                if (result != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.FullName)
                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.rememberMe
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                        _logger.LogInformation("User {UserId} logged in.", result.Id);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);

                        TempData["LoginSuccessMessage"] = "ورود شما با موفقیت انجام شد.";
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("", "نام کاربری یا رمز عبور نادرست است.");
                    return View(model);
            //    }
                //catch (Exception ex)
                //{
                //    _logger.LogError(ex, "Login error for {User}", model.userName);
                //    ModelState.AddModelError("", "خطایی در ورود رخ داد.");
                //    return View(model);
                //}
            }
        #endregion
        #region ForgotPassword
        [Route("ForgotPassword")]
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            await _userService.ExtractAndSaveLast4DigitsAsync(forgot.UserName);

            TempData["SuccessMessage"] = "کلمه عبور جدید به شماره موبایل ثبت‌شده شما پیامک شد.";

            return View();
        }
        #endregion
        
        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Cart");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
        #endregion
    }
}