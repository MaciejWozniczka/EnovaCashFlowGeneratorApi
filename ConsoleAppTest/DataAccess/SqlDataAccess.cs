using ConsoleAppTest.Models;
using Dapper;

namespace ConsoleAppTest.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        long ISqlDataAccess.Add<ModelType>(ModelType model)
        {
            throw new NotImplementedException();
        }

        Task<ICollection<ModelType>> ISqlDataAccess.GetAsync<ModelType>(string sql, DynamicParameters parameters, Tenant tenant)
        {
            throw new NotImplementedException();
        }
    }
}
