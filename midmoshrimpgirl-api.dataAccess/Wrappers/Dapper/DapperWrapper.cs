using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace midmoshrimpgirl_api.dataAccess.Wrappers.Dapper
{
    public class DapperWrapper : IDapperWrapper
    {
        private readonly string _connectionString;

        public DapperWrapper() { } //TODO: Remove 

        public DapperWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters)
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.QueryAsync<T>(
                $"[dbo].[{storedProcedureName}]",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }
    }
}
