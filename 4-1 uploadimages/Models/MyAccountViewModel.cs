using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UploadImages.data;

namespace _4_1_uploadimages.Models
{
    public class MyAccountViewModel
    {
        public User User { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}