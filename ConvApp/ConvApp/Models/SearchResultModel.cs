using ConvApp.ViewModels;
using System.Collections.Generic;

namespace ConvApp.Models
{
    public class SearchResultModel
    {
        public List<ProductModel> Products { get; set; }
        public List<PostingModel> Postings { get; set; }

        public static List<SearchResultViewModel> Populate(SearchResultModel model)
        {
            var result = new List<SearchResultViewModel>();
            foreach (var product in model.Products)
            {
                result.Add(new SearchResultViewModel
                {
                    Type = 1,
                    Id = product.Id,
                    Description = product.Name,
                    Image = product.Image
                });
            }

            foreach (var postings in model.Postings)
            {
                string desc;
                string image;
                if (postings.PostingType == (byte)PostingTypes.RECIPE)
                {
                    desc = postings.PostingNodes[0].Text;
                    image = postings.PostingNodes[2].Image;
                }
                else
                {
                    desc = postings.Products[0].Name + "에 대한 상품평";
                    image = postings.PostingNodes[2].Image;
                }

                result.Add(new SearchResultViewModel
                {
                    Type = 0,
                    Id = postings.Id,
                    Description = desc,
                    Image = image
                });
            }

            return result;
        }
    }
}
