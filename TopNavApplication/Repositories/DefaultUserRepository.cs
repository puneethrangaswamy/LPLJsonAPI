using Microsoft.Extensions.Options;
using TopNavApplication.Models.response;

namespace TopNavApplication.Repositories
{
    public sealed class DefaultUserRepository : RepositoryBase, IUserRepository
    {
        public DefaultUserRepository(IOptions<ConnectionStringsOptions> options)
            : base(options)
        {

        }

        public async Task<string> GetRoleByUserName(string userName)
        {
            string sqlQuery = $@"select 
                eg.id as entitlement_group_id, 
                eg.name as entitlement_group_name, 
                eg.description as entitlement_group_description 
                from entitlement_group eg
                join user_entitlement_mapping uem  on eg.id = uem.group_id
                join   appuser au on uem.user_id = au.id
                where (au.username = '{userName}')";

            string role;

            await using (var dbConnection = await OpenDbConnection())
            {
                await using (var dataReader = await ExecuteQueryAsync(sqlQuery, dbConnection))
                {
                    var rowAvailable = await dataReader.ReadAsync();

                    var groups = new List<EntitlementGroup>();

                    while (await dataReader.ReadAsync())
                    {
                        var entitlementGroup = new EntitlementGroup()
                        {
                            ID = GetValue(dataReader, 0, -1),
                            name = GetValue(dataReader, 1, string.Empty),
                            description = GetValue(dataReader, 2, string.Empty),
                        };

                        groups.Add(entitlementGroup);
                    }

                    var group = groups.SingleOrDefault();

                    role = group?.name ?? string.Empty;
                }
            }

            return role;
        }

        public async Task<bool> UserExists(string userName, string password)
        {
            string query = $"select exists(select 1 from appuser where username = '{userName}' and password = '{password}')";

            bool userExists = false;

            await using (var dbConnection = await OpenDbConnection())
            {
                await using (var dataReader = await ExecuteQueryAsync(query, dbConnection))
                {
                    var rowAvailable = await dataReader.ReadAsync();

                    if (rowAvailable)
                    {
                        userExists = (bool)dataReader[0];
                    }
                }
            }

            return userExists;
        }
    }
}
