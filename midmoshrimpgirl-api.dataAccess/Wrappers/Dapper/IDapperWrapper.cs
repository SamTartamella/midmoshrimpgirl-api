using Dapper;

namespace midmoshrimpgirl_api.dataAccess.Wrappers.Dapper
{
    public interface IDapperWrapper
    {
        public Task<IEnumerable<T>> ExecuteStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters);
    }
}
