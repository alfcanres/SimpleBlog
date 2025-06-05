using DataTransferObjects.DTO;
using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Client.Repository.PostType;
using WebAPI.Client.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class PostTypeController : Controller
    {
        private readonly IPostTypeRepository _repository;
        private readonly ILogger<PostTypeController> _logger;

        public PostTypeController(
            IPostTypeRepository repository,
            ILogger<PostTypeController> logger)
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
            return View(new PostTypeCreateDTO());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostTypeCreateDTO createModel)
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
                return RedirectToAction("Index", "PostType");
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

            PostTypeUpdateDTO PostTypeUpdateDTO = new PostTypeUpdateDTO
            {
                Id = response.Content.Id,
                Description = response.Content.Description,
                IsAvailable = response.Content.IsAvailable,                
            };


            return View(PostTypeUpdateDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PostTypeUpdateDTO editModel)
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
                return RedirectToAction("Index", "PostType");
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
        public async Task<ActionResult> Delete(PostTypeReadDTO postReadDTO)
        {

            var response = await _repository.DeleteAsync(postReadDTO.Id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "PostType");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(postReadDTO);
            }
        }
    }
}
