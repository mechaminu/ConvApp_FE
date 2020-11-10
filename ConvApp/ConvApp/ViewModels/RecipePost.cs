using ConvApp.ViewModels.Postings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class RecipePost : Post
    {
        public string Title { get; set; }           // 레시피 이름
        public string PostContent { get; set; }     // 레시피 소개

        public List<PostContentNode> RecipeNode { get; set; }   // 이미지 노드들
        public List<Product> ProductList { get; set; }          // 상품 목록들
    }
}
