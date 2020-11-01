using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class PostEntry
    {
        // 포스트 입력시 사용하는 뷰모델

        // 유저 입력으로 결정되는 값
        public PostType Type { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public List<MediaFile> PostImage { get; set; }
        // TODO
        // 상품목록, 별점, 조회수
        // 좋아요, 댓글, 신고 등 피드백 연계
    }
}
