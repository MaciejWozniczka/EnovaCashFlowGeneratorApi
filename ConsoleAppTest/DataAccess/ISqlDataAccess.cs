using ConsoleAppTest.Models;
using Dapper;

namespace ConsoleAppTest.DataAccess
{
    public interface ISqlDataAccess
    {
        long Add<ModelType>(ModelType model) where ModelType : class;
        Task<ICollection<ModelType>> GetAsync<ModelType>(string sql, DynamicParameters parameters = null, Tenant tenant = null) where ModelType : class;
    }
}
