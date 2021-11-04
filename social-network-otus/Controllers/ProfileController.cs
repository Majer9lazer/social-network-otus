using System.ComponentModel.DataAnnotations;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using social_network_otus.Repositories.Dapper.MySql;
using System.Threading.Tasks;

namespace social_network_otus.Controllers
{
    public class ProfileController : Controller
    {
        public async Task<IActionResult> Index([FromServices] IUserRepository userRepo)
        {
            var profiles = await userRepo.GetAll(0, 10);
            return View(profiles);
        }

        public IActionResult RandomUser()
        {
            return View();
        }

        [HttpGet("Profile/GetPaginated")]
        public async Task<IActionResult> GetPaginated([Required] int count, [Required] int skip, [FromServices] IUserRepository userRepo, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var result = await userRepo.GetAll(skip, count, ct);
            return Ok(result);
        }

        [HttpGet("Profile/Search")]
        public async Task<IActionResult> Search([Required] string userName, [Required] string userLastName, [FromServices] IUserRepository userRepo, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var result = await userRepo.GetByNameAndLastName(userName, userLastName, ct);
            return Ok(result);
        }
    }
}
