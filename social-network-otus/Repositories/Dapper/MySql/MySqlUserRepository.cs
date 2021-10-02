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
    }

    public class MySqlUserRepository : IUserRepository
    {
        private const string GetAllQueryTemplate = "select * from aspnetusers";
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
    }
}
