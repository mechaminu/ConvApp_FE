using System;
using System.Collections.Generic;
using System.IO;

namespace ConvApp.Models
{
    public class ProductModel
    {
        public static string[] StoreNames = { "", "GS25", "CU", "미니스톱", "세븐일레븐", "이마트24", "홈플러스 익스프레스" };
        public static string[] CategoryNames = { "", "도시락", "라면", "김밥", "즉석식품", "스낵", "음료", "주류", "생활용품", "기타" };

        public int Id { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get => StoreNames[this.StoreId]; }
        public int CategoryId { get; set; }
        public string CategoryName { get => CategoryNames[this.CategoryId]; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public string Image
        {
            get => ImageFilename != null
                ? Path.Combine(ApiManager.ImageEndPointURL, ImageFilename)
                : string.Empty;
        }
        public string ImageFilename { get; set; }

        public List<PostingModel> Postings { get; set; }
    }
}
