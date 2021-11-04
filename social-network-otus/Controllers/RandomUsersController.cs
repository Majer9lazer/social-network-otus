using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_network_otus.Data;
using social_network_otus.Extensions;
using social_network_otus.Services;
using System.Threading.Tasks;

namespace social_network_otus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RandomUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IRandomUserProvider _randomUserProvider;
        public RandomUsersController(ApplicationDbContext dbContext, IRandomUserProvider randomUserProvider)
        {
            _dbContext = dbContext;
            _randomUserProvider = randomUserProvider;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateUsers(int count, CancellationToken ct)
        {
            var users = await _randomUserProvider.GenerateAndSaveRandomUsers(count, User.Id(), _dbContext, ct);
            
            return Ok(users);
        }
    }
}
