
using Forum.Data.Models;
using Forum.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Web.ViewModels.Comments
{
    public class EditCommentsInputModel : IMapFrom<Comment>
    {
        public int PostId { get; set; }

        public int ParentId { get; set; }

        public string Content { get; set; }
    }
}
