using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_network_otus.Data;
using social_network_otus.Data.Models;
using social_network_otus.Models;
using System.Threading.Tasks;

namespace social_network_otus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseTokensController : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddToken([FromBody] FirebaseUserTokenModel model, [FromServices] ApplicationDbContext dbContext)
        {
            var userIpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            var firebaseUserToken = await dbContext.FirebaseUserTokens.FirstOrDefaultAsync(f => f.Token == model.Token);
            if (firebaseUserToken != null)
            {
                firebaseUserToken.UpdateDate = System.DateTime.Now;
                firebaseUserToken.AdditionalData = model.AdditionalData;
            }
            else
            {
                firebaseUserToken = new FirebaseUserToken()
                {
                    Token = model.Token,
                    IpAddress = userIpAddress,
                    CreateDate = System.DateTime.Now,
                    AdditionalData = model.AdditionalData
                };

                await dbContext.FirebaseUserTokens.AddAsync(firebaseUserToken);
            }

            await dbContext.SaveChangesAsync();

            return Ok(firebaseUserToken);
        }
    }
}
