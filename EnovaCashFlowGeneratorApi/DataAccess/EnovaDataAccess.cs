using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    public interface IEnovaDataAccess
    {
        Task<ICollection<ModelType>> GetAsync<ModelType>(string sql, string dbName, string connectionString, DynamicParameters parameters = null) where ModelType : class;
    }

    public class EnovaDataAccess : IEnovaDataAccess
    {
        public async virtual Task<ICollection<ModelType>> GetAsync<ModelType>(string sql, string dbName, string connectionString, DynamicParameters parameters = null) where ModelType : class
        {
            IEnumerable<ModelType> output;

            using (IDbConnection newConnection = new SqlConnection(connectionString))
            {
                newConnection.Open();
                newConnection.ChangeDatabase(dbName);
                output = await newConnection.QueryAsync<ModelType>(sql, parameters, transaction: null, 600);
            }

            return output.ToList();
        }
    }
}