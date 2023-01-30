using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TheForumHubMVC.Data.Enums;
using TheForumHubMVC.Data.Helpers;
using TheForumHubMVC.Data.Services;
using TheForumHubMVC.Data.ViewModels;
using TheForumHubMVC.Data.ViewModels.Admin;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionService _questionManager;
        private readonly ITagService _tagManager;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public QuestionsController(ILogger<QuestionsController> logger,
                              IQuestionService questionManager,
                              ITagService tagManager,
                              IMapper mapper,
                              UserManager<User> userManager)
        {
            _logger = logger;
            _questionManager = questionManager;
            _tagManager = tagManager;
            _mapper = mapper;
            _userManager = userManager;
        }
        #region Edit
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionManager.GetQuestionByIdAsync(id);
            if (question == null || question.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }
            ViewData["id"] = id;
            ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
            return View(_mapper.Map<QuestionVM>(question));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, QuestionVM model)
        {
            if(!ModelState.IsValid)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                return View(model);
            }
            if (model.TagsId.Count() > 5)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                ModelState.AddModelError("TagsId", "Maximum tags 5.");
                return View(model);
            }
            var question = (await _questionManager.GetQuestionByIdAsync(id));
            var userId = _userManager.GetUserId(User);
            if (question == null || question.UserId != userId)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                ModelState.AddModelError("", "Error");
                return View(model);
            }
            model.UserId = userId;
            await _questionManager.UpdateQuestionAsync(id, model);
            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion
        #region Rating
        [Authorize]
        public async Task<IActionResult> Add(int id)
        {
            var questionRating = new QuestionRatingVM()
            {
                UserId = _userManager.GetUserId(User),
                QuestionId = id,
                Rating = 1
            };
            await _questionManager.AddRatingAsync(questionRating);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize]
        public async Task<IActionResult> Remove(int id)
        {
            var questionRating = new QuestionRatingVM()
            {
                UserId = _userManager.GetUserId(User),
                QuestionId = id,
                Rating = -1
            };
            await _questionManager.AddRatingAsync(questionRating);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        #endregion
        #region Answer
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Answer(int id, string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }
            var user = await _userManager.GetUserAsync(User);
            var answerVM = new AnswerVM()
            {
                Content = answer,
                QuestionId = id,
                UserId = user.Id,
                UrlHelper = Url,
                Notifications = user.OnNotification
            };
            await _questionManager.AddAnswerAsync(answerVM);
            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion
        #region All Questions
        public async Task<IActionResult> All(string categorie = "", int page = 1, int pageSize = 10)
        {
            IEnumerable<Question> questions = (await _questionManager.GetQuestionsAsync());
            if (!(pageSize == 10 || pageSize == 25 || pageSize == 50)) pageSize = 10;
            ViewBag.Paggination = new PagginationVM()
            {
                Action = "All",
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(questions.Count() / (double)pageSize)
            };
            switch (categorie.ToLower())
            {
                case "rating":
                    categorie = "Rating";
                    questions = questions.OrderByDescending(q => q?.Ratings.Sum(r => r.Rating) ?? 0);
                    break;
                case "views":
                    categorie = "Views";
                    questions = questions.OrderByDescending(q => q?.Views ?? 0);
                    break;
                default:
                    categorie = "Date";
                    questions = questions.OrderByDescending(q => q.Asked);
                    break;

            }
            ViewBag.Categorie = categorie;
            return View(questions.Skip((page - 1) * pageSize).Take(pageSize));
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionManager.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();

            var questionUserRating = question.Ratings.FirstOrDefault(rating => rating.UserId == _userManager.GetUserId(User));
            if (questionUserRating != null)
            {
                ViewData["QuestionUserRating"] = questionUserRating.Rating;
            }

            await _questionManager.AddViewsAsync(new ViewsVM()
            {
                Session = HttpContext.Session,
                QuestionId = id
            });
            return View(question);
        }
        #endregion
        #region Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(QuestionVM question)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                return View(question);
            }
            question.Title = question.Title.Trim();
            question.Description = question.Description.Trim();

            if (question.Title.Length < 5)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                ModelState.AddModelError("Title", "The Title minimum length is 5 characters.");
                return View(question);
            }
            if (question.Description.Length < 20)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                ModelState.AddModelError("Description", "The Description minimum length is 20 characters.");
                return View(question);
            }

            if (question.TagsId.Count() > 5)
            {
                ViewData["Tags"] = new SelectList(await _tagManager.GetTagsAsync(), "Id", "Name");
                ModelState.AddModelError("TagsId", "Maximum tags 5.");
                return View(question);
            }
            question.UserId = (await _userManager.GetUserAsync(User)).Id;

            var responseQuestion = await _questionManager.CreateQuestionAsync(question);
            return RedirectToAction("Details", new { id = responseQuestion.Id });
        }
        #endregion
        #region Create Tag
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTag()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTag(TagVM tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            var responseQuestion = await _tagManager.CreateTagAsync(tag);
            return RedirectToAction("All");
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionManager.GetQuestionByIdAsync(id);
            if (question == null || question.UserId != _userManager.GetUserId(User)) { return NotFound(); }
            return View(question);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var question = await _questionManager.GetQuestionByIdAsync(id);
            if (question == null || question.UserId != _userManager.GetUserId(User)) { return NotFound(); }
            await _questionManager.DeleteQuestionAsync(id);
            return RedirectToAction(nameof(All));
        }
        #endregion
        [HttpGet, Authorize]
        public async Task<IActionResult> Report(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Report(int id, ReportVM model)
        {
            if (!ModelState.IsValid) { ViewBag.Id = id; return View(model); }
            model.UserId = _userManager.GetUserId(User);
            model.TypeId = id;
            await _questionManager.Report(model);
            return RedirectToAction(nameof(Details), new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> Tags(string search, int page = 1, int pageSize = 10)
        {
            if (!(pageSize == 10 || pageSize == 25 || pageSize == 50)) pageSize = 10;
            var tags = await _tagManager.GetTagsAsync();

            ViewBag.TagsPaggination = new TagsPagginationVM()
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(tags.Count() / (double)pageSize),
                Search = search
            };
            if (search != null) search = search.Trim();
            if (string.IsNullOrWhiteSpace(search)) 
            {
                var totalPages = (int)Math.Ceiling(tags.Count() / (double)pageSize);
                if (page > totalPages) ViewBag.TagsPaggination.Page = totalPages;
                return View(tags.Skip((page - 1) * pageSize).Take(pageSize)); 
            }

            ViewData["Search"] = search;

            var searchTags = tags.Where(t => t.Name.ToLower().Contains(search.ToLower()));
            var searchTotalPages = (int)Math.Ceiling(searchTags.Count() / (double)pageSize);
            if (page > searchTotalPages) ViewBag.TagsPaggination.Page = searchTotalPages;
            ViewBag.TagsPaggination.TotalPages = searchTotalPages;

            return View(searchTags.Skip((page - 1) * pageSize).Take(pageSize));
        }
        public async Task<IActionResult> Search(string search)
        {
            var questions = await _questionManager.GetQuestionsAsync();

            ViewData["Title"] = "Questions";
            if (search == null || search.Length == 0) return RedirectToAction("All");
            search = search.Trim();
            if (search.Length == 0) return RedirectToAction("All");
            ViewData["Title"] = search;
            return View(questions.Where(u => u.Title.ToLower().Contains(search.ToLower()) || u.Description.ToLower().Contains(search.ToLower())));
        }
        public async Task<IActionResult> Home()
        {
            var questions = (await _questionManager.GetQuestionsAsync()).OrderByDescending(q => q.Asked).Take(10);
            return View(questions);
        }
        public async Task<IActionResult> Filter(string tag, string categorie = "", int page = 1, int pageSize = 10)
        {
            var questions = (await _questionManager.GetQuestionsAsync()).Where(q => q.Question_Tags.FirstOrDefault(qt => qt.Tag.Name == tag) != null);
            
            switch (categorie.ToLower())
            {
                case "rating":
                    categorie = "Rating";
                    questions = questions.OrderByDescending(q => q?.Ratings.Sum(r => r.Rating) ?? 0);
                    break;
                case "views":
                    categorie = "Views";
                    questions = questions.OrderByDescending(q => q?.Views);
                    break;
                default:
                    categorie = "Date";
                    questions = questions.OrderByDescending(q => q.Asked);
                    break;
            }
            var filter = new FilterVM()
            {
                Tag = await _tagManager.GetTagByNameAsync(tag),
                Categorie = categorie,
                Questions = questions
            };
            if (!(pageSize == 10 || pageSize == 25 || pageSize == 50)) pageSize = 10;
            ViewBag.Paggination = new PagginationVM()
            {
                Action = "Filter",
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(questions.Count() / (double)pageSize),
                Categorie = categorie,
                Tag = tag
            };
            return View(filter);
        }
    }
}