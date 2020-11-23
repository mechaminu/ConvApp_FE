using System;
using System.Collections.Generic;

namespace ConvApp.Models
{
    public class ProductDTO
    {
        public static string[] StoreNames = { "", "GS25", "CU", "미니스톱", "세븐일레븐", "이마트24", "홈플러스 익스프레스" };
        public static string[] CategoryNames = { "", "도시락", "라면", "김밥", "즉석식품", "스낵", "음료", "주류", "생활용품", "기타" };

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string StoreName { get => ProductDTO.StoreNames[this.StoreId]; }
        public string CategoryName { get => ProductDTO.CategoryNames[this.CategoryId]; }

        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }

        public List<PostingDTO> Postings { get; set; }
    }
}
