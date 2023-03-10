using Forum.Data.Models;
using Forum.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Web.ViewModels.Categories
{
    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<PostInCategoryViewModel> ForumPosts { get; set; }
    }
}
