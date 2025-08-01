using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Client.Repository;
using WebAPI.Client.Repository.Account;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.ViewModels;


namespace WebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IApplicationUserInfoRepository _applicationUserInfoRepository;
        private readonly IJwtTokenAuthenticationHandler _jwtTokenAuthenticationHandler;


        public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository, IJwtTokenAuthenticationHandler jwtTokenAuthenticationHandler, IApplicationUserInfoRepository applicationUserInfoRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _jwtTokenAuthenticationHandler = jwtTokenAuthenticationHandler;
            _applicationUserInfoRepository = applicationUserInfoRepository;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountSignInDTO loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var response = await _accountRepository.LoginAsync(loginViewModel);

            if (response.Status == ResponseStatus.Success)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginViewModel.Email),
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                    };



                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Response.Cookies.Append("AuthToken", response.Content.Token, new CookieOptions { HttpOnly = true, Secure = true });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(loginViewModel);
            }
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(AccountCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateDTO);
            }

            var response = await _accountRepository.RegisterAsync(userCreateDTO);

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("RegisterSucess", "Account");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(userCreateDTO);
            }
        }


        public IActionResult RegisterSucess()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Index", "Home");
        }


        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<IActionResult> EditProfile()
        {
            var userEmail = User.Identity?.Name;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _applicationUserInfoRepository.GetByEmailAsync(userEmail);
            
            if (response.Status ==  ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(response.Content);
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ApplicationUserInfoUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDTO);
            }

            var userEmail = User.Identity?.Name;

            if(userEmail != updateDTO.UserName)
            {
                ModelState.AddModelError(string.Empty, "Email mismatch.");
                return View(updateDTO);
            }
            

            var response = await _applicationUserInfoRepository.UpdateAsync(updateDTO);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "MoodType");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(updateDTO);
            }
        }


        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AccountChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userEmail = User.Identity?.Name;

            if (userEmail != model.Email)
            {
                ModelState.AddModelError(string.Empty, "Email mismatch.");
                return View(model);
            }


            var response = await _accountRepository.UpdatePasswordAsync(model);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "MoodType");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(model);
            }
        }



    }
}
