using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.Model
{
    public class ImageModel
    {
        public int studentId { get; set; }
        public IFormFile image { get; set; }
    }
}
