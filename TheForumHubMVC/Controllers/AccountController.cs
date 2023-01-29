using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheForumHubMVC.Data.Services;
using TheForumHubMVC.Data.ViewModels.Account;
using TheForumHubMVC.Data.ViewModels.Mail;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _mailManager;
        private readonly IQuestionService _questionManager;
        private readonly IAnswerService _answerManager;
        private readonly IAccountService _accountManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<User> signInManager,
                                 UserManager<User> userManager,
                                 IEmailService mailManager,
                                 IQuestionService questionManager,
                                 IAnswerService answerManager,
                                 IAccountService accountManager,
                                 IMapper mapper,
                                 ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mailManager = mailManager;
            _questionManager = questionManager;
            _answerManager = answerManager;
            _accountManager = accountManager;
            _mapper = mapper;
            _logger = logger;
        }
        #region Profile
        [AllowAnonymous]
        [Route("{action}/{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByNameAsync(id);
            if (user == null || await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return NotFound();
            }
            var userProfile = new ProfileVM()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate,
                LogoImg = user.LogoImg,
                Notification = user.OnNotification,
                Answers = (await _answerManager.GetAnswersAsync()).Where(a => a.UserId == user.Id),
                Questions = (await _questionManager.GetQuestionsAsync()).Where(q => q.UserId == user.Id),
                QuestionRatings = (await _questionManager.GetQuestionsRatingAsync()).Where(qr => qr.UserId == user.Id),
                AnswerRatings = (await _answerManager.GetAnswersRatingAsync()).Where(ar => ar.UserId == user.Id)
            };
            ViewData["Photo"] = "data:image;base64," + Convert.ToBase64String(userProfile.LogoImg);
            return View(userProfile);
        }
        [Route("{action}")]
        [Route("")]
        public async Task<IActionResult> Profile()
        {
            var user = (await _userManager.GetUserAsync(User));
            var userProfile = new ProfileVM()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate,
                LogoImg = user.LogoImg,
                Notification = user.OnNotification,
                Answers = (await _answerManager.GetAnswersAsync()).Where(a => a.UserId == user.Id),
                Questions = (await _questionManager.GetQuestionsAsync()).Where(q => q.UserId == user.Id),
                QuestionRatings = (await _questionManager.GetQuestionsRatingAsync()).Where(qr => qr.UserId == user.Id),
                AnswerRatings = (await _answerManager.GetAnswersRatingAsync()).Where(ar => ar.UserId == user.Id)
        };
            ViewData["Photo"] = "data:image;base64," + Convert.ToBase64String(userProfile.LogoImg);
            return View(userProfile);
        }
        #endregion
        #region Login
        [AllowAnonymous, HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);

            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            ModelState.AddModelError("", "Incorrect username or password");
            return View(model);
        }
        #endregion
        #region Register
        [AllowAnonymous, HttpGet]
        [Route("[action]")]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                RegistrationDate = DateTime.Now,
                VerificationToken = Guid.NewGuid().ToString()
            };
            using (var stream = new FileStream("D:\\projects\\TheForumHubMVC\\TheForumHubMVC\\wwwroot\\defaultprofile.jpg", FileMode.Open))
            {
                var LogoImg = new byte[stream.Length];
                stream.Read(LogoImg, 0, (int)stream.Length);
                user.LogoImg = LogoImg;
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var message = new MailVM()
                {
                    Subject = "Confirm your email",
                    Body = $"<a href=\"{Url.ActionLink(action: "Confirm", controller: "Account", new { uid = user.Id, token = user.VerificationToken })}\">Verificate</a>",
                    Email = model.Email
                };
                _mailManager.SendMail(message);
                _logger.LogInformation($"Registered new account. '{model.Username}'");
                return View("Confirm");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        #endregion
        #region Edit
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return View(new EditVM()
            {
                Username = user.UserName,
                Notifications = user.OnNotification
            });
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (!ModelState.IsValid) return View(model);

            user.UserName = model.Username;
            user.OnNotification = model.Notifications;
            
            if (model.NewLogoImg != null)
            {

                var contentType = model.NewLogoImg.ContentType;
                if (!contentType.Contains("image"))
                {
                    ModelState.AddModelError("NewLogoImg", "Incorrect image format.");
                    return View(model);
                }
                using (var photo = model.NewLogoImg.OpenReadStream())
                {
                    if (photo.Length > 524288)
                    {
                        ModelState.AddModelError("NewLogoImg", "Maximum photo size 512KB");
                    }
                    user.LogoImg = new byte[photo.Length];
                    photo.Read(user.LogoImg, 0, (int)photo.Length);
                }
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Username", "Username already taken.");
                return View(model);
            }
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Profile));
        }
        #endregion
        #region Questions / Answers
        [Route("[action]"), AllowAnonymous]
        public async Task<IActionResult> Questions(string username)
        {
            if (username == null) return NotFound();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || await _userManager.IsInRoleAsync(user, Roles.Admin)) return NotFound();
            ViewData["Title"] = username + " questions";
            return View((await _questionManager.GetQuestionsAsync()).Where(q => q.UserId == user.Id));
        }
        [Route("[action]"), AllowAnonymous]
        public async Task<IActionResult> Answers(string username)
        {
            if (username == null) return NotFound();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || await _userManager.IsInRoleAsync(user, Roles.Admin)) return NotFound();
            ViewData["Title"] = username + " answers";
            return View((await _questionManager.GetQuestionsAsync()).Where(q => q.Answers.FirstOrDefault(a => a.User.UserName == username) != null).OrderByDescending(q => q.Asked));
        }
        #endregion
        #region Change password
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userManager.ChangePasswordAsync(await _userManager.GetUserAsync(User), model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return View(model);
            }
            return RedirectToAction(nameof(Profile));
        }
        #endregion
        #region Change email
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> ChangeEmail()
        {
            return View();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);

            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordCheck)
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return View(model);
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);

            var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, token);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("NewEmail", "Email already taken.");
                return View(model);
            }
            
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(Profile));
        }
        #endregion
        #region Delete
        [HttpGet]
        [Route("{action}")]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(_mapper.Map<DeleteVM>(user));
        }
        [HttpPost]
        [Route("{action}")]
        public async Task<IActionResult> Delete(DeleteVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.ConfirmPassword);
            if(!checkPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Incorrect password.");
                return View(model);
            }

            await _accountManager.DeleteAsync(user.Id);
            await _userManager.DeleteAsync(user);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Home", "Questions");
        }
        #endregion
        [Route("[action]"), AllowAnonymous]
        public async Task<IActionResult> Search(string search, int page = 1, int pageSize = 10)
        {
            var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin);

            var users = _userManager.Users
                                    .Include(u => u.Answers)
                                    .ThenInclude(a => a.Ratings)
                                    .Include(u => u.Questions)
                                    .ThenInclude(q => q.Ratings)
                                    .Where(u => !admins.Contains(u));
            

            if (!(pageSize == 10 || pageSize == 25 || pageSize == 50)) pageSize = 10;
            ViewBag.UserPaggination = new SearchUserPagginationVM()
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(users.Count() / (double)pageSize),
                Search = search
            };

            ViewData["Title"] = "Users";
            if (search == null || search.Length == 0) return View(users.Skip((page - 1) * pageSize).Take(pageSize));
            search = search.Trim();
            if (search.Length == 0) return View(users);
            ViewData["Title"] = search;
            ViewData["Search"] = true;

            var newUsers = users.Where(u => u.UserName.ToLower().Contains(search.ToLower())).Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.UserPaggination.TotalPages = (int)Math.Ceiling(newUsers.Count() / (double)pageSize);

            return View(newUsers);
        }
        [AllowAnonymous, HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Confirm(string uid, string token)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (user.EmailConfirmed == true || user.VerificationToken != token) return BadRequest("Error");
            user.EmailConfirmed = true;
            user.VerificatedAt = DateTime.Now;
            user.VerificationToken = null;
            await _userManager.AddToRoleAsync(user, Roles.User);
            return View();
        }
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToRoute("default");
        }
    }
}
