using DataTransferObjects.DTO;
using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.ViewModels;

namespace WebApp.Controllers
{
    public class ApplicationUserInfoController : Controller
    {
        private readonly IApplicationUserInfoRepository _repository;
        private readonly ILogger<ApplicationUserInfoController> _logger;

        public ApplicationUserInfoController(
            IApplicationUserInfoRepository repository,
            ILogger<ApplicationUserInfoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


    }
}
