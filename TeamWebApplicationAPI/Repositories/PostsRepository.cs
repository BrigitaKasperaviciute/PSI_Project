﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TeamWebApplicationAPI.Data.Database;
using TeamWebApplicationAPI.Models;
using TeamWebApplicationAPI.Repositories.Interfaces;

namespace TeamWebApplicationAPI.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly ApplicationDBContext _db;
        public ActionDelegate<Post> UpdateAndSaveDelegate { get; set; }
        public PostsRepository(ApplicationDBContext db)
        {
            _db = db;

            UpdateAndSaveDelegate = async (originalPost, post) =>
            {
                await UpdatePostAsync(originalPost, post);
            };
        }

        public async Task<IEnumerable<Post>> GetPostsByCourseAsync(int? courseId)
        {
            if (courseId == null)
                throw new ArgumentNullException(nameof(courseId));

            return await _db.Posts
                .Where(p => p.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int? postId)
        {
            if (postId == null)
                throw new ArgumentNullException(nameof(postId));

            return await _db.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task InsertPostAsync<T>(T post) where T : Post?
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            await _db.Posts.AddAsync(post);
            await SaveAsync();
        }

        public async Task DeletePostByIdAsync(int? postId)
        {
            if (postId == null)
                throw new ArgumentNullException(nameof(postId));

            var post = await _db.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

            if (post != null)
            {
                _db.Posts.Remove(post);
                await SaveAsync();
            }
        }

        public async Task DeletePostAsync<T>(T post) where T : Post?
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _db.Posts.Remove(post);
            await SaveAsync();
        }

        public async Task UpdatePostAsync<T>(T originalPost, T post) where T : Post?
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            originalPost.CreationDate = DateTime.Now;
            originalPost.Name = post.Name;
            originalPost.IsVisible = post.IsVisible;
            originalPost.PostType = post.PostType;

            if (originalPost is TextPost textPost && post is TextPost textPostToUpdate)
            {
                textPost.TextContent = textPostToUpdate.TextContent;
            }
            if (originalPost is FilePost filePost && post is FilePost filePostToUpdate)
            {
                filePost.FileName = filePostToUpdate.FileName;
            }
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}