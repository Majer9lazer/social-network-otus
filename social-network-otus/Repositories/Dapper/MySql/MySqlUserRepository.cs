using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using social_network_otus.Data.Models;
using social_network_otus.Services;

namespace social_network_otus.Repositories.Dapper.MySql
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAll(CancellationToken ct = default);
        Task<int> Add(ApplicationUser user, CancellationToken ct = default);
    }

    public class MySqlUserRepository : IUserRepository
    {
        private const string GetAllQueryTemplate = "select * from aspnetusers";

        private const string InsertUserTemplate =
            "insert into `aspnetusers` (BirthDate, Gender, RangeOfInterests, CityName, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, UserLastName) " +
            "values(@BirthDate, @Gender, @RangeOfInterests, @CityName, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount, @UserLastName);";
        
        private readonly IConnectionStringFactory _connectionStringFactory;

        public MySqlUserRepository(IConnectionStringFactory connectionStringFactory)
        {
            _connectionStringFactory = connectionStringFactory;
        }

        public async Task<List<ApplicationUser>> GetAll(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await using var dbConnection = new MySqlConnection(_connectionStringFactory.ConnectionString);
            await dbConnection.OpenAsync(ct);

            return (await dbConnection.QueryAsync<ApplicationUser>(GetAllQueryTemplate)).ToList();
        }

        public async Task<int> Add(ApplicationUser user, CancellationToken ct = default)
        {
            await using var dbConnection = new MySqlConnection(_connectionStringFactory.ConnectionString);
            await dbConnection.OpenAsync(ct);
            return await dbConnection.ExecuteAsync(InsertUserTemplate, user);
        }
    }
}
