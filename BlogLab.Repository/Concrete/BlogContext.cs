using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

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