using ConvApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class ProductModel
    {
        public static string[] StoreNames = { "", "GS25", "CU", "미니스톱", "세븐일레븐", "이마트24", "홈플러스 익스프레스" };
        public static string[] CategoryNames = { "", "도시락", "라면", "김밥", "즉석식품", "스낵", "음료", "주류", "생활용품", "기타" };

        public long Id { get; set; }
        public long StoreId { get; set; }
        public string StoreName { get => StoreNames[this.StoreId]; }
        public long CategoryId { get; set; }
        public string CategoryName { get => CategoryNames[this.CategoryId]; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }

        public string Image
        {
            get => ImageFilename != null
                ? Path.Combine(ApiManager.ImageEndPointURL, ImageFilename)
                : string.Empty;
        }
        public string ImageFilename { get; set; }

        public List<PostingModel> Postings { get; set; }

        public async static Task<ProductViewModel> Populate(ProductModel model)
        {
            var reviews = new ObservableCollection<ReviewViewModel>();
            var recipes = new ObservableCollection<RecipeViewModel>();

            foreach (var e in model.Postings)
            {
                var post = await PostingModel.Populate(e);

                if (post is RecipeViewModel)
                    recipes.Add(post as RecipeViewModel);
                else
                    reviews.Add(post as ReviewViewModel);
            }

            return new ProductViewModel
            {
                Id = model.Id,
                StoreId = model.StoreId,
                CategoryId = model.CategoryId,
                CreatedDate = model.CreatedDate,
                Image = model.Image,
                Name = model.Name,
                Price = model.Price,
                Rank = "9999",
                Rate = $"{(reviews.Any() ? reviews.Average(r => r.Rating) : 0)}",
                Calory = "9999",
                ReviewList = reviews,
                RecipeList = recipes
            };
        }
    }
}
