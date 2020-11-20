using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ConvApp.ViewModels
{
    public class Feedback : INotifyPropertyChanged
    {
        private int curPage = 0;
        private bool scrollEnded = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public Feedback()
        {
            BaseTime = DateTime.UtcNow;
        }

        public byte Type { get; set; }
        public int Id { get; set; }

        public int ViewCount { get; set; }                  // 해당 포스트의 세부페이지 조회 수
        public int LikeCount { get => Likes.Count; }        // 해당 포스트의 전체 좋아요 수
        public int CommentCount { get => Comments.Count; }   // 해당 포스트의 코멘트 수

        public DateTime BaseTime { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }

        public bool IsLiked
        {
            get => Likes.Find((l) => l.Creator.Id == App.User.Id) != null;
        }

        public async void Refresh()
        {
            BaseTime = DateTime.UtcNow;
            curPage = 0;

            Comments = await ApiManager.GetComments(Type, Id, BaseTime, curPage);
            Likes = await ApiManager.GetLikes(Type, Id);
        }

        public void AddComment()
        {

        }
        
        public async void ScrollComment()
        {
            if (scrollEnded)
                return;

            var nextList = await ApiManager.GetComments(Type, Id, BaseTime, ++curPage);
            if (nextList == null || nextList.Count == 0)
            {
                curPage--;
                scrollEnded = true;
                return;
            }
            Comments.AddRange(nextList);
        }

        public async Task ToggleLike()
        {
            if (!IsLiked)
                Likes = await ApiManager.PostLike(Type, Id);
            else
                Likes = await ApiManager.DeleteLike(Type, Id);

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Likes"));
        }

        public async void RefreshLike()
        {
            Likes = await ApiManager.GetLikes(Type, Id);
        }
    }
}
