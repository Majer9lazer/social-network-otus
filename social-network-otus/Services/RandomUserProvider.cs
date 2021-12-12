using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using social_network_otus.Data;
using social_network_otus.Data.Models;

#if LOCAL
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RandomNameGeneratorLibrary;
using social_network_otus.Hubs;
using System;
using System.Collections.Concurrent;
#endif

namespace social_network_otus.Services
{
    public interface IRandomUserProvider
    {
        ValueTask<ApplicationUser[]> GenerateRandomUsers(int userCount, string callerId, CancellationToken ct = default);
        ValueTask<List<ApplicationUser>> GenerateAndSaveRandomUsers(int userCount, string callerId, ApplicationDbContext dbContext, CancellationToken ct = default);
    }

#if LOCAL
    public class RandomUserProvider : IRandomUserProvider
    {
        private readonly PersonNameGenerator _personGenerator;
        private readonly PlaceNameGenerator _placeGenerator;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IHubContext<ProfileHub, IProfileHubClient> _profileHub;

        public RandomUserProvider(PersonNameGenerator personGenerator,
            PlaceNameGenerator placeGenerator,
            IHubContext<ProfileHub, IProfileHubClient> profileHub,
            IServiceScopeFactory<IPasswordHasher<ApplicationUser>> scopeFactory)
        {
            _personGenerator = personGenerator;
            _placeGenerator = placeGenerator;
            _passwordHasher = scopeFactory.ScopedInstance;
            _profileHub = profileHub;
        }

        public async ValueTask<ApplicationUser[]> GenerateRandomUsers(int userCount, string callerId, CancellationToken ct = default)
        {
            var users = new ApplicationUser[userCount];
            ct.ThrowIfCancellationRequested();

            if (userCount > 10_000)
            {
                var concurrentBag = new ConcurrentBag<ApplicationUser>();
                Parallel.For(0, userCount, async (i, a) =>
                {
                    if (ct.IsCancellationRequested)
                    {
                        a.Stop();
                    }

                    var user = CreateRandomUser();
                    concurrentBag.Add(user);

                    await _profileHub.Clients.User(callerId).ReceiveCreationProgress(i);

                });

                users = concurrentBag.ToArray();
            }
            else
            {
                for (var i = 0; i < userCount; i++)
                {
                    var user = CreateRandomUser();
                    users[i] = user;
                    await _profileHub.Clients.User(callerId).ReceiveCreationProgress(i);
                }
            }

            return users;
        }

        public async ValueTask<List<ApplicationUser>> GenerateAndSaveRandomUsers(int userCount, string callerId, ApplicationDbContext dbContext,
            CancellationToken ct = default)
        {
            var users = new List<ApplicationUser>(userCount);
            ct.ThrowIfCancellationRequested();
            await dbContext.Database.ExecuteSqlRawAsync("ALTER TABLE AspNetUsers DISABLE KEYS;", ct);
            for (var j = userCount; j > 0; j -= 10_000)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                var concurrentBag = new ConcurrentBag<ApplicationUser>();
                Parallel.For(0, j < 10_000 ? j : 10_000, async (i, a) =>
                {
                    if (ct.IsCancellationRequested)
                    {
                        a.Stop();
                    }

                    var user = CreateRandomUser();
                    concurrentBag.Add(user);
                    await _profileHub.Clients.User(callerId).ReceiveCreationProgress(i);

                });

                users.AddRange(concurrentBag);

                await dbContext.AddRangeAsync(concurrentBag, ct);
                await dbContext.SaveChangesAsync(ct);

            }

            await dbContext.Database.ExecuteSqlRawAsync("DELETE\r\nFROM AspNetUsers\r\nWHERE Id IN (\r\n    SELECT Id\r\n    FROM (\r\n             SELECT ROW_NUMBER() OVER (\r\n                 PARTITION BY email\r\n                 ORDER BY email) AS row_num\r\n             FROM AspNetUsers\r\n         ) t\r\n    WHERE row_num > 1\r\n);", ct);
            await dbContext.Database.ExecuteSqlRawAsync("ALTER TABLE AspNetUsers ENABLE KEYS;", ct);
            return users;
        }

        private ApplicationUser CreateRandomUser()
        {
            var user = new ApplicationUser();
            var hashedPassword = _passwordHasher.HashPassword(user, "test");
            var lastName = _personGenerator.GenerateRandomLastName();
            var firstName = _personGenerator.GenerateRandomFirstName();

            user.Id = Guid.NewGuid().ToString();
            user.BirthDate = DateTime.Now.AddYears(-18);
            user.CityName = _placeGenerator.GenerateRandomPlaceName();
            user.UserName = firstName;
            user.NormalizedUserName = firstName.ToUpperInvariant();
            user.UserLastName = lastName;
            user.Email = $"{firstName}_{lastName}@rnd.com";
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            user.PasswordHash = hashedPassword;
            user.Gender = "rnd";
            user.RangeOfInterests = "huiload";
            return user;
        }
    }
#endif
}
