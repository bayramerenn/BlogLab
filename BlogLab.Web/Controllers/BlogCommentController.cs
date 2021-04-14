﻿using BlogLab.Models.BlogComment;
using BlogLab.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCommentController : ControllerBase
    {
        private readonly IBlogCommentRepository _blogCommentRepository;

        public BlogCommentController(IBlogCommentRepository blogCommentRepository)
        {
            _blogCommentRepository = blogCommentRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogCommentCreate blogCommentCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(f => f.Type == JwtRegisteredClaimNames.NameId).Value);

            var createdBlogComment = await _blogCommentRepository.UpsertAsync(blogCommentCreate, applicationUserId);

            return Ok(createdBlogComment);
        }

        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetAll(int blogId)
        {
            var blogComments = await _blogCommentRepository.GetAllAsync(blogId);

            return Ok(blogComments);
        }

        [Authorize]
        [HttpDelete("{blogCommentId}")]
        public async Task<IActionResult> Delete(int blogCommentId)
        {
            int applicationUserId = int.Parse(User.Claims.First(f => f.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundBlogComment = await _blogCommentRepository.GetAsync(blogCommentId);

            if (foundBlogComment == null)
            {
                return BadRequest("Comment does not exists");
            }
            if (foundBlogComment.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _blogCommentRepository.DeleteAsync(blogCommentId);

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("This comment was not created by the current user");
            }
        }
    }
}