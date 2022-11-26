using API.FB.Core.FBAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FB.Core.Entities
{
    public class MediaPost : BaseEntity
    {
        public int MediaID { get; set; }

        public int? PostID { get; set; }

        public string? Token { get; set; }

        public string? Image { get; set; }

        public string? Video { get; set; }

        public bool? IsImage { get; set; }

    }
}
