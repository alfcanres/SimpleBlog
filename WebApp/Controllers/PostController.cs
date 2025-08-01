using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.PostVote;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using WebAPI.Client.Repository;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.Repository.MoodType;
using WebAPI.Client.Repository.Post;
using WebAPI.Client.Repository.PostType;
using WebAPI.Client.ViewModels;
using WebApp.Models;



namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;
        private readonly ILogger<PostController> _logger;
        private readonly IPostTypeRepository _postTypeRepository;
        private readonly IMoodTypeRepository _moodTypeRepository;
        private readonly IApplicationUserInfoRepository _applicationUserInfoRepository;
        private readonly IJwtTokenAuthenticationHandler _jwtTokenAuthenticationHandler;
        private readonly int _currentUserID;

        public PostController(
            IPostRepository repository,
            IPostTypeRepository postTypeRepository,
            IMoodTypeRepository moodTypeRepository,
            IApplicationUserInfoRepository applicationUserInfoRepository,
            IJwtTokenAuthenticationHandler jwtTokenAuthenticationHandler,
            ILogger<PostController> logger)
        {
            _repository = repository;
            _logger = logger;
            _postTypeRepository = postTypeRepository;
            _moodTypeRepository = moodTypeRepository;
            _applicationUserInfoRepository = applicationUserInfoRepository;
            _jwtTokenAuthenticationHandler = jwtTokenAuthenticationHandler;

            string authToken = _jwtTokenAuthenticationHandler.GetToken();

            if (!string.IsNullOrEmpty(authToken))
            {
                _currentUserID = Helpers.ClaimsHelper.GetApplicationUserInfoId(authToken);
            }

        }

        public async Task<IActionResult> Index(PagerParams pager = null)
        {

            var response = await _repository.GetPublishedPagedAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public async Task<IActionResult> LoadMore(int page = 1, int recordsPerPage = 10, string searchKeyWord = null)
        {
            var pager = new PagerParams { CurrentPage = page };
            
            var response = await _repository.GetPublishedPagedAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
                return Unauthorized();

        
            return PartialView("_PostLoadMore", response.Content.List);
        }

        public async Task<IActionResult> MyPosts(PagerParams pager = null)
        {

            var response = await _repository.GetAllPagedByUserAsync(_currentUserID, pager);

            ViewBag.NavBarModel = new NavbarViewModel
            {
                RecordsPerPage = response.Content.RecordsPerPage,
                SearchKeyWord = response.Content.SearchKeyWord,
                ShowCreateNewButton = true,
                ActionName = "MyPosts",
                ControllerName = "Post"
            };

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }


        public async Task<IActionResult> Create()
        {
            var responseStatus = await LoadDropDowns();

            if (responseStatus == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(new PostCreateDTO()
            {
                ApplicationUserInfoId = _currentUserID
            }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateDTO createDTO)
        {


            if (!ModelState.IsValid)
            {
                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                return View(createDTO);
            }

            var response = await _repository.CreateAsync(createDTO);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("MyPosts", "Post");
            }
            else
            {
                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }


                return View(createDTO);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {

            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Content.ApplicationUserInfoId != _currentUserID)
            {
                return RedirectToAction("MyPosts", "Post");
            }

            var responseStatus = await LoadDropDowns();

            if (responseStatus == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            PostUpdateDTO PostUpdateDTO = new PostUpdateDTO
            {
                Id = response.Content.Id,
                ApplicationUserInfoId = response.Content.ApplicationUserInfoId,
                MoodTypeId = response.Content.MoodTypeId,
                PostTypeId = response.Content.PostTypeId,
                Title = response.Content.Title,
                Text = response.Content.Text,

            };


            return View(PostUpdateDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PostUpdateDTO editModel)
        {


            if (!ModelState.IsValid)
            {
                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                return View(editModel);
            }

            var response = await _repository.UpdateAsync(editModel);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("MyPosts", "Post");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                return View(editModel);
            }
        }

        public async Task<IActionResult> View(int id)
        {

            var postResp = await _repository.GetByIdAsync(id);
            var commentsResp = await _repository.GetPostCommentsAsync(id);
            var votesResp = await _repository.GetPostVotesAsync(id);


            if (
                postResp.Status == ResponseStatus.Unauthorized
                ||
                commentsResp.Status == ResponseStatus.Unauthorized
                ||
                votesResp.Status == ResponseStatus.Unauthorized
                )
            {
                return RedirectToAction("Logout", "Account");
            }

            var post = postResp.Content;

            var postReadviewModel = new PostReadViewModel()
            {
                Post = new PostListDTO
                {
                    ApplicationUserInfoId = post.ApplicationUserInfoId,
                    Id = post.Id,
                    Title = post.Title,
                    Text = post.Text,
                    UserName = post.UserName,
                    ProfilePic = post.ProfilePic,
                    PostType = post.PostType,
                    MoodType = post.MoodType,
                    PublishDate = post.PublishDate,
                    IsPublished = post.IsPublished
                },
                Comments = commentsResp.Content.List,
                Votes = votesResp.Content
            };


            return PartialView(postReadviewModel);
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
        public async Task<ActionResult> Delete(PostReadDTO postReadDTO)
        {


            var response = await _repository.DeleteAsync(postReadDTO.Id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("MyPosts", "Post");
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


        public async Task<IActionResult> Publish(int id)
        {

            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response.Content);
        }

        [HttpPost]
        public async Task<ActionResult> Publish(PostReadDTO readDTO)
        {


            var response = await _repository.ApproveAsync(readDTO.Id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("MyPosts", "Post");
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


        public async Task<IActionResult> CreateComment()
        {
            var responseStatus = await LoadDropDowns();

            if (responseStatus == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            return View(new PostCreateDTO()
            {
                ApplicationUserInfoId = _currentUserID
            }
            );
        }

        [HttpPost]
        public async Task<JsonResult> CreateComment(PostCommentCreateDTO createDTO)
        {

            var response = await _repository.CreateCommentAsync(createDTO);

            return Json(new
            {
                status = response.Status.ToString(),
                messages = response.MessageList,
                data = response.Content
            });

        }

        public async Task<IActionResult> ListComments(int PostId)
        {

            var response = await _repository.GetPostCommentsAsync(PostId);


            return PartialView("_PostComments", response.Content.List);
        }


        [HttpPost]
        public async Task<JsonResult> SubmitVote(SubmitVoteDTO submitVoteDTO)
        {
            submitVoteDTO.ApplicationUserInfoId = _currentUserID;

            var response = await _repository.VoteAsync(submitVoteDTO);

            return Json(new
            {
                status = response.Status.ToString(),
                messages = response.MessageList,
                data = response.Content
            });

        }


        public async Task<IActionResult> ShowVotes(int PostId)
        {

            var response = await _repository.GetPostVotesAsync(PostId);


            return PartialView("_PostVotes", response.Content);
        }

        private async Task<ResponseStatus> LoadDropDowns()
        {
            var types = await _postTypeRepository.GetAllByIsAvailableAsync(true);
            var moods = await _moodTypeRepository.GetIsAvailableAsync(true);

            if (moods.Status != ResponseStatus.Unauthorized && types.Status != ResponseStatus.Unauthorized)
            {
                ViewBag.MoodTypes = new SelectList(moods.Content.List, "Id", "Mood");
                ViewBag.PostTypes = new SelectList(types.Content.List, "Id", "Description");
            }

            return moods.Status == ResponseStatus.Unauthorized || types.Status == ResponseStatus.Unauthorized ? ResponseStatus.Unauthorized : ResponseStatus.Success;

        }


    }
}
