using BlogLab.Models.BlogComment;
using BlogLab.Repository.Abstract;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Repository.Concrete
{
    public class BlogCommentRepository : IBlogCommentRepository
    {
        private readonly IConfiguration _config;

        public BlogCommentRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int blogCommentId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                        "BlogComment_Delete",
                        new { BlogCommentId = blogCommentId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }

            return affectedRows;
        }

        public async Task<List<BlogComment>> GetAllAsync(int blogId)
        {
            IEnumerable<BlogComment> blogComments;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                blogComments = await connection.QueryAsync<BlogComment>(
                        "BlogComment_GetAll",
                        new { BlogId = blogId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            return blogComments.ToList();
        }

        public async Task<BlogComment> GetAsync(int blogCommentId)
        {
            BlogComment blogComment;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                blogComment = await connection.QueryFirstOrDefaultAsync<BlogComment>(
                        "BlogComment_Get",
                        new { BlogCommentId = blogCommentId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            return blogComment;
        }

        public async Task<BlogComment> UpsertAsync(BlogCommentCreate blogCommentCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("BlogCommentId", typeof(int));
            dataTable.Columns.Add("ParentBlogCommentId", typeof(int));
            dataTable.Columns.Add("BlogId", typeof(int));
            dataTable.Columns.Add("Content", typeof(string));

            dataTable.Rows.Add(blogCommentCreate.BlogCommmentId, blogCommentCreate.ParentBlogCommentId,
                blogCommentCreate.BlogId, blogCommentCreate.Content
                );

            int? newBlogCommentId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newBlogCommentId = await connection.ExecuteScalarAsync<int?>(
                        "BlogComment_Upsert",
                        new
                        {
                            BlogComment = dataTable.AsTableValuedParameter("dbo.BlogCommentType")
                        },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }

            newBlogCommentId = newBlogCommentId ?? blogCommentCreate.BlogId;

            var blog = await GetAsync(newBlogCommentId.Value);

            return blog;
        }
    }
}