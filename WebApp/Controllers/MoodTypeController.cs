using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Client.Repository.MoodType;
using WebAPI.Client.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class MoodTypeController : Controller
    {

        private readonly IMoodTypeRepository _repository;
        private readonly ILogger<MoodTypeController> _logger;

        public MoodTypeController(
            IMoodTypeRepository repository, 
            ILogger<MoodTypeController> logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;

        }

        public async Task<IActionResult> Index(PagerParams pager = null)
        {
           

            var response = await _repository.GetPagedAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public IActionResult Create()
        {
            return View(new MoodTypeCreateDTO());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoodTypeCreateDTO createModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createModel);
            }
                       

            var response = await _repository.CreateAsync(createModel);

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

                return View(createModel);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {


            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            MoodTypeUpdateDTO moodTypeUpdateDTO = new MoodTypeUpdateDTO
            {
                Id = response.Content.Id,
                Mood = response.Content.Mood,
                IsAvailable = response.Content.IsAvailable
            };


            return View(moodTypeUpdateDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MoodTypeUpdateDTO editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            var response = await _repository.UpdateAsync(editModel);

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

                return View(editModel);
            }
        }

        public async Task<IActionResult> View(int id)
        {



            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            return View(response.Content);
        }


        public async Task<IActionResult> Delete(int id)
        {


            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response.Content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(MoodTypeReadDTO readDTO)
        {


            var response = await _repository.DeleteAsync(readDTO.Id);

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

                return View(readDTO);
            }
        }


    }
}
