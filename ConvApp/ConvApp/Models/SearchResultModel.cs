using System.Collections.Generic;

namespace ConvApp.Models
{
    public class SearchResultModel
    {
        public List<ProductModel> Products { get; set; }
        public List<PostingModel> Postings { get; set; }
    }
}
