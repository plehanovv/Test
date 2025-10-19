using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Test.Domain.Interfaces;

namespace Test.Infrastructure.Persistence;

public class PostgresConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public PostgresConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("TestDb") ?? throw new ArgumentNullException("ConnectionString not found");
    }
    
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}