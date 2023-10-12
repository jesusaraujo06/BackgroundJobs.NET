using Microsoft.Data.SqlClient;
using System.Data;

namespace CustomerSearch.Data;

public class DapperDbContext
{
    private readonly IConfiguration Configuration;
    private readonly string? connectionString;
    public DapperDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
        connectionString = Configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
