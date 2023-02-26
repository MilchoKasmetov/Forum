namespace Forum.Web.Controllers
{
    using System.Threading.Tasks;

    using Forum.Data.Models;
    using Forum.Services.Data;
    using Forum.Web.ViewModels.Comments;
    using Forum.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(
            ICommentsService commentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var parentId =
                input.ParentId == 0 ?
                    (int?)null :
                    input.ParentId;

            if (parentId.HasValue)
            {
                if (!this.commentsService.IsInPostId(parentId.Value, input.PostId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.commentsService.Create(input.PostId, userId, input.Content, parentId);
            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }

        public IActionResult Edit(int id)
        {
            var model = this.commentsService.GetById<EditCommentsInputModel>(id);
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EditCommentsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.commentsService.UpdateAsync(id, input);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {

            await this.commentsService.Delete(id);

            return this.Redirect("/");
        }
    }
}
