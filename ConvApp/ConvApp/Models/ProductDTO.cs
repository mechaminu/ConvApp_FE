using System;
using System.Collections.Generic;

namespace ConvApp.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }

        public List<PostingDTO> Postings { get; set; }
    }
}
