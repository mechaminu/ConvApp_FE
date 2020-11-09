using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
  public class ReviewPost : Post
    {
        public double Rating { get; set; }                  // 평점 (0 ~ 10, 정수 >> 0 ~ 5, 0.5단위 소수)
        public string PostContent { get; set; }             // 본문
        public List<ImageSource> PostImage { get; set; }    // 이미지 배열
    }
}
