using Dapper;
using Microsoft.Data.SqlClient;

namespace DataAccessManager
{
    public static class DapperDataAccessManager
    {
        private static string _connectionString = "";
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
        public static async Task<List<T>> QueryList<T>(string query)
        {
            using var _dbContext = new SqlConnection(_connectionString);
            await _dbContext.OpenAsync();
            var result = await _dbContext.QueryAsync<T>(query);
            return result.ToList();
        }

        public static async Task<T> QueryObject<T>(string query)
        {
            using var _dbContext = new SqlConnection(_connectionString);
            await _dbContext.OpenAsync();
            var result = await _dbContext.QueryFirstOrDefaultAsync<T>(query);
            return result;
        }
    }
}
