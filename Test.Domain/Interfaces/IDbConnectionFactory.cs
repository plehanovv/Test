using System.Data;

namespace Test.Domain.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}