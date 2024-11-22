using Dapper;

namespace midmoshrimpgirl_api.dataAccess.Wrappers.Dapper
{
    public class DapperWrapper : IDapperWrapper
    {
        public Task<IEnumerable<T>> ExecuteStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
