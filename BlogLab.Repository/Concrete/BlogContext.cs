using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BlogLab.Repository.Concrete
{
    public class BlogContext
    {
        public SqlConnection sqlConnection;
        private readonly IConfiguration _config;

        public BlogContext()
        {
            sqlConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }
}