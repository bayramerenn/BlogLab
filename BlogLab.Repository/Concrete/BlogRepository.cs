using BlogLab.Models.Blog;
using BlogLab.Repository.Abstract;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Repository.Concrete
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IConfiguration _config;

        public BlogRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int blogId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                        "Blog_Delete",
                        new { BlogId = blogId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }

            return affectedRows;
        }

        public async Task<PageResults<Blog>> GetAllAsync(BlogPaging blogPaging)
        {
            var result = new PageResults<Blog>();

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync("Blog_GetAll",
                    new
                    {
                        Offset = (blogPaging.Page - 1) * blogPaging.PageSize,
                        PageSize = blogPaging.PageSize
                    },
                        commandType: System.Data.CommandType.StoredProcedure
                      ))
                {
                    result.Items = multi.Read<Blog>();
                    result.TotalCount = multi.ReadFirst<int>();
                }
            }

            return result;
        }

        public async Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId)
        {
            IEnumerable<Blog> blog;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                blog = await connection.QueryAsync<Blog>(
                        "Blog_GetByUserId",
                        new { ApplicationUserId = applicationUserId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            return blog.ToList();
        }

        public async Task<List<Blog>> GetAllFamousAsync()
        {
            IEnumerable<Blog> famouesBlogs;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                famouesBlogs = await connection.QueryAsync<Blog>(
                        "Blog_GetAllFamous",
                        new { },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            return famouesBlogs.ToList();
        }

        public async Task<Blog> GetAsync(int blogId)
        {
            Blog blog;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                blog = await connection.QueryFirstOrDefaultAsync<Blog>(
                    "Blog_Get",
                    new { BlogId = blogId },
                    commandType: CommandType.StoredProcedure);
            }

            return blog;
        }

        public async Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("BlogId", typeof(int));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Content", typeof(string));
            dataTable.Columns.Add("PhotoId", typeof(int));

            dataTable.Rows.Add(blogCreate.BlogId, blogCreate.Title, blogCreate.Content, blogCreate.PhotoId);

            int? newBlogId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newBlogId = await connection.ExecuteScalarAsync<int?>(
                    "Blog_Upsert",
                    new { Blog = dataTable.AsTableValuedParameter("dbo.BlogType"), ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure
                    );
            }

            newBlogId = newBlogId ?? blogCreate.BlogId;

            Blog blog = await GetAsync(newBlogId.Value);

            return blog;
        }
    }
}