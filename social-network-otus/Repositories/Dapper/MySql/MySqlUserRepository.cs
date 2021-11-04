using System;
using Dapper;
using MySql.Data.MySqlClient;
using social_network_otus.Data.Models;
using social_network_otus.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace social_network_otus.Repositories.Dapper.MySql
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAll(CancellationToken ct = default);
        Task<List<ApplicationUser>> GetAll(int skip, int count, CancellationToken ct = default);
        Task<int> Add(ApplicationUser user, CancellationToken ct = default);
        Task<IReadOnlyCollection<ApplicationUser>> GetByNameAndLastName(string name, string lastName, CancellationToken ct = default);
    }

    public class MySqlUserRepository : IUserRepository
    {
        private const string GetAllQueryTemplate = "select * from AspNetUsers ";

        private const string InsertUserTemplate =
            "insert into `AspNetUsers` (Id, BirthDate, Gender, RangeOfInterests, CityName, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, UserLastName) " +
            "values(@Id, @BirthDate, @Gender, @RangeOfInterests, @CityName, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount, @UserLastName);";

        private const string GetByFirstNameAndLastNameQueryTemplate = "SELECT * FROM `AspNetUsers` WHERE UserName LIKE '{0}%' and UserLastName like '{1}%' ORDER BY Id;";

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

        public async Task<List<ApplicationUser>> GetAll(int skip, int count, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await using var dbConnection = new MySqlConnection(_connectionStringFactory.ConnectionString);
            await dbConnection.OpenAsync(ct);
            var sqlQuery = string.Concat(GetAllQueryTemplate, $"LIMIT {count} OFFSET {skip};");
            return (await dbConnection.QueryAsync<ApplicationUser>(sqlQuery)).ToList();
        }

        public async Task<int> Add(ApplicationUser user, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await using var dbConnection = new MySqlConnection(_connectionStringFactory.ConnectionString);
            await dbConnection.OpenAsync(ct);

            return await dbConnection.ExecuteAsync(InsertUserTemplate, user, commandTimeout: int.MaxValue);
        }

        public async Task<IReadOnlyCollection<ApplicationUser>> GetByNameAndLastName(string name, string lastName, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await using var dbConnection = new MySqlConnection(_connectionStringFactory.ConnectionString);
            await dbConnection.OpenAsync(ct);

            return (await dbConnection.QueryAsync<ApplicationUser>(string.Format(GetByFirstNameAndLastNameQueryTemplate,
                name, lastName))).AsList();

        }
    }
}
