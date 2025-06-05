using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.ViewModels;

namespace WebApp.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IApplicationUserInfoRepository _repository;
        private readonly ILogger<ApplicationUserInfoController> _logger;

        public AuthorsController(
            IApplicationUserInfoRepository repository, 
            ILogger<ApplicationUserInfoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(PagerParams pager = null)
        {


            var response = await _repository.GetPagedByActiveAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public async Task<IActionResult> View(int userId)
        {

            var response = await _repository.GetByIdAsync(userId);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            return View(response.Content);
        }

    }
}
