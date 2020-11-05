using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.Model
{
   public class FeedBack
    {
        public Boolean Like { get; set; } // 좋아요
        public int LikeCnt { get; set; } // 좋아요 수
        public int View { get; set; }// 조회 수
        public string Comment { get; set; } // 코멘트 
        public int CommentNumber { get; set; }// 코멘트 수

    }
}
