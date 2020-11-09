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
        public List<Node> RecipeNode { get; set; }  // 노드들
    }
}
