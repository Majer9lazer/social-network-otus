using Microsoft.AspNetCore.Mvc;
using social_network_otus.Repositories.Dapper.MySql;
using System.Threading.Tasks;

namespace social_network_otus.Controllers
{
    public class ProfileController : Controller
    {
        public async Task<IActionResult> Index([FromServices] IUserRepository userRepo)
        {
            var profiles = await userRepo.GetAll();
            return View(profiles);
        }
    }
}
