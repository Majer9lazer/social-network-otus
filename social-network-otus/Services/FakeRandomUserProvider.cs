using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using social_network_otus.Data;
using social_network_otus.Data.Models;

namespace social_network_otus.Services
{
    public class FakeRandomUserProvider : IRandomUserProvider
    {
        public ValueTask<ApplicationUser[]> GenerateRandomUsers(int userCount, string callerId, CancellationToken ct = default)
        {
            return new ValueTask<ApplicationUser[]>();
            throw new System.NotImplementedException();
        }

        public ValueTask<List<ApplicationUser>> GenerateAndSaveRandomUsers(int userCount, string callerId, ApplicationDbContext dbContext,
            CancellationToken ct = default)
        {
            return new ValueTask<List<ApplicationUser>>();
        }
    }
}