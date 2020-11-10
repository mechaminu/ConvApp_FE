using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.ViewModels
{
   public class FeedBack
    {
        public Boolean Liked { get; set; } // 현재 유저가 해당 포스트에 좋아요 눌렀는지 여부
        public int LikeCount { get; set; } // 해당 포스트의 전체 좋아요 수

        public int ViewCount { get; set; }// 해당 포스트의 세부페이지 조회 수

        public int CmtCount { get; set; }// 해당 포스트의 코멘트 수
        public string Cmt { get; set; } // 해당 포스트의 코멘트 객체 

    }
}
