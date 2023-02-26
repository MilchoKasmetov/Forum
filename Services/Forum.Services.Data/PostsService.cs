namespace Forum.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Forum.Data.Common.Repositories;
    using Forum.Data.Models;
    using Forum.Services.Mapping;
    using Forum.Web.ViewModels.Posts;
    using Microsoft.EntityFrameworkCore;

    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository; 

        public PostsService(IDeletableEntityRepository<Post> postsRepository )
        {
            this.postsRepository = postsRepository;
          
        }

        public async Task<int> CreateAsync(string title, string content, int categoryId, string userId)
        {
            var post = new Post
            {
                CategoryId = categoryId,
                Content = content,
                Title = title,
                UserId = userId,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
            return post.Id;
        }

        
        public async Task Delete(int id)
        {
            var post = await this.postsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0)
        {
            var query = this.postsRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var post = this.postsRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return post;
        }

        public int GetCountByCategoryId(int categoryId)
        {
            return this.postsRepository.All().Count(x => x.CategoryId == categoryId);
        }

        public async Task UpdateAsync(int id, EditPostsInputModel input)
        {
            var post = await this.postsRepository.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            post.Title = input.Title;
            post.Content = input.Content;
            post.CategoryId = input.CategoryId;
            await this.postsRepository.SaveChangesAsync();
        }
    }
}
