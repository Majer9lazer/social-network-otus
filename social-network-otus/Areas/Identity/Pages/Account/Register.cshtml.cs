using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using social_network_otus.Data.Models;
using social_network_otus.Models;
using social_network_otus.Repositories.Dapper.MySql;

namespace social_network_otus.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserPasswordStore<ApplicationUser> _passwordStore;
        private readonly IEnumerable<IPasswordValidator<ApplicationUser>> _passwordValidators;
        private readonly IEnumerable<IUserValidator<ApplicationUser>> _userValidators;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IUserRepository _userRepository;
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators, IUserStore<ApplicationUser> userStore, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _userValidators = userValidators;
            _userRepository = userRepository;
            _passwordStore = userStore as IUserPasswordStore<ApplicationUser> ??
                             throw new ArgumentNullException(nameof(userStore),
                                 $"{nameof(userStore)} doesn't support {nameof(IUserPasswordStore<ApplicationUser>)}");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [MaxLength(250)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [MaxLength(250)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(255)]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [MaxLength(10)]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Required]
            [MaxLength(255)]
            [Display(Name = "Range Of Interests")]
            public string RangeOfInterests { get; set; }

            [Required]
            [Display(Name = "Birthday")]
            public DateTime BirthDate { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Password confirm")]
            [Compare("Password", ErrorMessage = "Passwords differ")]
            public string ConfirmPassword { get; set; }
        }

        public Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null, CancellationToken ct = default)
        {
            returnUrl ??= Url.Action("Index", "Profile");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = Input.FirstName,
                    UserLastName = Input.LastName,
                    Email = Input.Email,
                    BirthDate = Input.BirthDate,
                    CityName = Input.City,
                    RangeOfInterests = Input.RangeOfInterests,
                    Gender = Input.Gender
                };

                var result = await CreateAsync(user, Input.Password, ct);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await SendEmailAsync(user, returnUrl, ct);
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken ct = default)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            var result = await UpdatePasswordHash(user, password, ct);
            if (!result.Succeeded)
            {
                return result;
            }
            return await CreateAsync(user, ct).ConfigureAwait(false);
        }

        private async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken ct)
        {

            //await _userManager.UpdateSecurityStampInternal(user);
            var result = await ValidateUserAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            await _userManager.UpdateNormalizedUserNameAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);

            //await _userStore.CreateAsync(user, ct);
            var rowsAffected = await _userRepository.Add(user, ct);
            _logger.LogInformation("Rows affected while creating user = {0}", rowsAffected);
            return IdentityResult.Success;
        }

        private async Task<IdentityResult> ValidateUserAsync(ApplicationUser user)
        {
            var errors = new List<IdentityError>();
            foreach (var v in _userValidators)
            {
                var result = await v.ValidateAsync(_userManager, user);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                _logger.LogWarning(13, "User validation failed: {errors}.", string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async Task<IdentityResult> UpdatePasswordHash(ApplicationUser user, string newPassword, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var validate = await ValidatePasswordAsync(user, newPassword, ct);
            if (!validate.Succeeded)
            {
                return validate;
            }

            var hash = newPassword != null ? _passwordHasher.HashPassword(user, newPassword) : null;
            await _passwordStore.SetPasswordHashAsync(user, hash, ct);
            //await UpdateSecurityStampInternal(user);
            return IdentityResult.Success;
        }

        private async Task<IdentityResult> ValidatePasswordAsync(ApplicationUser user, string password, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var errors = new List<IdentityError>();
            var isValid = true;
            foreach (var v in _passwordValidators)
            {
                ct.ThrowIfCancellationRequested();
                var result = await v.ValidateAsync(_userManager, user, password);
                if (!result.Succeeded)
                {
                    if (result.Errors.Any())
                    {
                        errors.AddRange(result.Errors);
                    }

                    isValid = false;
                }
            }
            if (!isValid)
            {
                _logger.LogWarning(14, "User password validation failed: {errors}.", string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async ValueTask SendEmailAsync(ApplicationUser user, string returnUrl, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
        }
    }
}
