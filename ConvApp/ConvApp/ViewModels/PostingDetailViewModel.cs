using ConvApp.Models;
using System;
using System.Collections.Generic;

namespace ConvApp.ViewModels
{
    public class PostingDetailViewModel
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public List<ProductDTO> Products { get; set; }
        public FeedbackViewModel Feedback { get; set; }
    }

    public class PostNode
    {
        public string NodeImage { get; set; }                   // 이미지 Url
        public string NodeString { get; set; }                  // 이미지 에대한 설명
    }

    public class ReviewPostingViewModel : PostingDetailViewModel
    {
        public double Rating { get; set; }                      // 평점 (0 ~ 10, 정수 >> 0 ~ 5, 0.5단위 소수)
        public string PostContent { get; set; }                 // 본문
        public string PostImage { get; set; }                   // 이미지 Url
    }

    public class RecipePostingViewModel : PostingDetailViewModel
    {
        public string Title { get; set; }                       // 레시피 이름
        public string PostContent { get; set; }                 // 레시피 소개
        public List<PostNode> RecipeNode { get; set; }          // 이미지 노드들
        public List<ProductDTO> ProductList { get; set; }       // 상품 목록들
    }
}
