using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class Post 
    {
        //[Required(ErrorMessage ="ID của bài đăng phải được thiết lập")]
        public Guid PostID { get; set; }
        //[Required(ErrorMessage ="Bài đăng không được để trống")]
        //[StringLength(500, MinimumLength = 0, ErrorMessage = "Số lượng kí tự không được quá 500")]
        public Guid UserID { get; set; }

        public string? Content { get; set; }

        public string? Media { get; set; }
        
    }
}
