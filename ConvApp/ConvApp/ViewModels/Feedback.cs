using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConvApp.ViewModels
{
    public class FeedBacks : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public byte Type { get; set; }
        public int Id { get; set; }

        public int ViewCount { get; set; }// 해당 포스트의 세부페이지 조회 수
        public int LikeCount { get; set; } // 해당 포스트의 전체 좋아요 수
        public int CommentCount { get; set; }// 해당 포스트의 코멘트 수

        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }

    }
}
