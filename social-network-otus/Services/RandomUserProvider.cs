using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RandomNameGeneratorLibrary;
using social_network_otus.Data;
using social_network_otus.Data.Models;
using social_network_otus.Hubs;
using social_network_otus.Repositories.Dapper.MySql;

namespace social_network_otus.Services
{
    public interface IRandomUserProvider
    {
        ValueTask<ApplicationUser[]> GenerateRandomUsers(int userCount, string callerId, CancellationToken ct = default);
        ValueTask<List<ApplicationUser>> GenerateAndSaveRandomUsers(int userCount, string callerId, ApplicationDbContext dbContext, CancellationToken ct = default);
    }
    public class RandomUserProvider : IRandomUserProvider
    {
        private readonly PersonNameGenerator _personGenerator;
        private readonly PlaceNameGenerator _placeGenerator;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IHubContext<ProfileHub, IProfileHubClient> _profileHub;
        private readonly IUserRepository _userRepository;
        public RandomUserProvider(PersonNameGenerator personGenerator,
            PlaceNameGenerator placeGenerator,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IHubContext<ProfileHub, IProfileHubClient> profileHub, IUserRepository userRepository)
        {
            _personGenerator = personGenerator;
            _placeGenerator = placeGenerator;
            _passwordHasher = passwordHasher;
            _profileHub = profileHub;
            _userRepository = userRepository;
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

                await dbContext.Users.BulkInsertAsync(concurrentBag, ct);
                await dbContext.BulkSaveChangesAsync(operation =>
                {
                    operation.BatchSize = 10_000;

                },ct);

            }

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
}
