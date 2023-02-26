using Forum.Data.Common.Repositories;
using Forum.Data.Models;
using Forum.Services.Mapping;
using Forum.Web.ViewModels.Comments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services.Data
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(
            IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task Create(int postId, string userId, string content, int? parentId = null)
        {
            var comment = new Comment
            {
                Content = content,
                ParentId = parentId,
                PostId = postId,
                UserId = userId,
            };
            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var comment = await this.commentsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public bool IsInPostId(int commentId, int postId)
        {
            var commentPostId = this.commentsRepository.All().Where(x => x.Id == commentId)
                .Select(x => x.PostId).FirstOrDefault();
            return commentPostId == postId;
        }

        public async Task UpdateAsync(int id, EditCommentsInputModel input)
        {
            var comment = await this.commentsRepository.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            comment.Content = input.Content;

            await this.commentsRepository.SaveChangesAsync();
        }

        public T GetById<T>(int id)
        {
            var post = this.commentsRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return post;
        }
    }
}
