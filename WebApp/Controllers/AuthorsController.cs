using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.Repository.Post;
using WebAPI.Client.ViewModels;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IApplicationUserInfoRepository _repository;
        private readonly ILogger<AuthorsController> _logger;
        private readonly IPostRepository _postRepository;

        public AuthorsController(
            IApplicationUserInfoRepository repository,
            IPostRepository postRepository,
            ILogger<AuthorsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _postRepository = postRepository;
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

        public async Task<IActionResult> LoadMore(int page = 1, int recordsPerPage = 10, string searchKeyWord = null)
        {
            var pager = new PagerParams
            {
                CurrentPage = page,
                RecordsPerPage = recordsPerPage,
                SearchKeyWord = searchKeyWord ?? ""
            };

            var response = await _repository.GetPagedByActiveAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
                return Unauthorized();

    
            return PartialView("_AuthorLoadMore", response.Content.List);
        }

        public async Task<IActionResult> View(int userId)
        {
            

            var authorResponse = await _repository.GetByIdAsync(userId);
            var postPagedListResponse = await _postRepository.GetPublishedPagedByUserAsync(userId, new PagerParams
            {
                CurrentPage = 1,
                RecordsPerPage = 10
            });

            if (authorResponse.Status == ResponseStatus.Unauthorized || postPagedListResponse.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            AuthorViewModel authorViewModel = new AuthorViewModel
            {
                Author = authorResponse.Content,
                PostsPagedList = postPagedListResponse.Content
            };



            return View(authorViewModel);
        }


        public async Task<IActionResult> LoadMorePosts(
            int userId, 
            int page)
        {
            var pager = new PagerParams { CurrentPage = page };

            var response = await _postRepository.GetPublishedPagedByUserAsync(userId, pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return PartialView("_PostLoadMore", response.Content.List);
        }


    }
}
