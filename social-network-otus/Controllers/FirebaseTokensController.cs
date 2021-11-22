﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_network_otus.Data;
using social_network_otus.Data.Models;
using System.Threading.Tasks;

namespace social_network_otus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseTokensController : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddToken(string token, [FromServices] ApplicationDbContext dbContext)
        {
            var userIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var firebaseUserToken = await dbContext.FirebaseUserTokens.FirstOrDefaultAsync(f => f.Token == token);
            if (firebaseUserToken != null)
            {
                firebaseUserToken.UpdateDate = System.DateTime.Now;
            }
            else
            {
                firebaseUserToken = new FirebaseUserToken()
                {
                    Token = token,
                    IpAddress = userIpAddress,
                    CreateDate = System.DateTime.Now
                };

                await dbContext.FirebaseUserTokens.AddAsync(firebaseUserToken);
            }

            await dbContext.SaveChangesAsync();

            return Ok(firebaseUserToken);
        }
    }
}
