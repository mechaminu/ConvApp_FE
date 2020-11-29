using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.Models
{
    public class SearchResultModel
    {
        public List<ProductModel> Products { get; set; } 
        public List<PostingModel> Postings { get; set; }
    }
}
