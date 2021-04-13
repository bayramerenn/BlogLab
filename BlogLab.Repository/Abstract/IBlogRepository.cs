﻿using BlogLab.Models.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogLab.Repository.Abstract
{
    public interface IBlogRepository
    {
        public Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId);

        public Task<PageResults<Blog>> GetAllAsync(BlogPaging blogPaging);

        public Task<Blog> GetAsync(int blogId);

        public Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId);

        public Task<List<Blog>> GetAllFamousAsync();

        public Task<int> DeleteAsync(int blogId);
    }
}