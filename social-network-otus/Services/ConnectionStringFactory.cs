using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace social_network_otus.Services
{
    public interface IConnectionStringFactory
    {
        string ConnectionStringName { get; }
        string ConnectionString { get; }
    }

    public class MySqlConnectionStringFactory : IConnectionStringFactory
    {
        private readonly IConfiguration _configuration;
        public MySqlConnectionStringFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionStringName => "MySqlConnection";

        public string ConnectionString => _configuration.GetConnectionString(ConnectionStringName);

    }
}
