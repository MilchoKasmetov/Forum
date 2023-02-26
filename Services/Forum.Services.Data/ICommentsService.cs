namespace Forum.Services.Data
{
    using Forum.Web.ViewModels.Comments;
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task Create(int postId, string userId, string content, int? parentId = null);

        bool IsInPostId(int commentId, int postId);

        Task UpdateAsync(int id, EditCommentsInputModel input);

        Task Delete(int id);

        T GetById<T>(int id);
    }
}
