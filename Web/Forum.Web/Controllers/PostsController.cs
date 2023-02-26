namespace Forum.Web.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Forum.Data.Models;
    using Forum.Services.Data;
    using Forum.Services.Mapping;
    using Forum.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
      

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public IActionResult ById(int id)
        {
            var postViewModel = this.postsService.GetById<PostViewModel>(id);
            if (postViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(postViewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
            var viewModel = new PostCreateInputModel
            {
                Categories = categories,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var postId = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, user.Id);
            
            return this.RedirectToAction(nameof(this.ById), new { id = postId });
        }


        public async Task<IActionResult> Edit(int id)
        {

            var user = await this.userManager.GetUserAsync(this.User);
            var currentPost = this.postsService.GetById<PostViewModel>(id);

            if(user.Email != currentPost.UserUserName)
            {
                return this.RedirectToAction("Index", "Home");
            }
            var model = this.postsService.GetById<EditPostsInputModel>(id);
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
            model.Categories = categories;

            return this.View(model);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EditPostsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.postsService.UpdateAsync(id, input);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var currentPost = this.postsService.GetById<PostViewModel>(id);

            if (user.Email != currentPost.UserUserName)
            {
                return this.RedirectToAction("Index", "Home");
            }

            await this.postsService.Delete(id);

            return this.Redirect("/");
        }
    }
}
