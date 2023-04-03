using Microsoft.Extensions.Options;
using Npgsql;

namespace TopNavApplication.Repositories
{
    public class RepositoryBase
    {
        private readonly ConnectionStringsOptions _options;

        protected RepositoryBase(IOptions<ConnectionStringsOptions> options)
        {
            _options = options.Value;
        }

        protected async Task<NpgsqlDataReader> ExecuteQueryAsync(string sqlQuery, NpgsqlConnection dbConnection)
        {
            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            var dataReader = await command.ExecuteReaderAsync();

            return dataReader;
        }

        protected async Task<NpgsqlConnection> OpenDbConnection()
        {
            var dbConnection = new NpgsqlConnection(_options.Default);

            await dbConnection.OpenAsync();

            return dbConnection;
        }

        protected TValue GetValue<TValue>(NpgsqlDataReader dataReader, int index, TValue defaultValue)
        {
            var rawValue = dataReader[index];
            TValue typedValue;

            if (rawValue != null && rawValue != DBNull.Value)
            {
                typedValue = (TValue)rawValue;
            }
            else
            {
                typedValue = defaultValue;
            }

            return typedValue;
        }
    }
}
