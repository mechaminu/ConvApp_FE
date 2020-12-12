using ConvApp.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        public CommentViewModel()
        {
            DeleteCmtCommand = new Command(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("댓글 삭제","댓글을 삭제하시겠습니까?","예","아니오"))
                {
                    await ApiManager.DeleteComment(this.Id);
                    RefreshParent?.Invoke();
                }
            });
        }

        public long Id { get; set; }
        public bool IsChild { get; set; }
        public DateTime Date { get; set; }

        public UserBriefModel Creator { get; set; }
        public bool Owned { get => App.User != null && Creator.Id == App.User.Id; }
        public bool NotOwned { get => !Owned; }
        public string Text { get; set; }

        public FeedbackViewModel Feedback { get; set; }

        public ICommand ChildCmtCommand { set; get; }
        public ICommand DeleteCmtCommand { set; get; }

        // 자식 요소인 댓글이 부모 요소인 FeedbackView의 새로고침을 Invoke 해야하는 상황
        // Dependency Injection을 쓰고 싶지만... 일단 delegate 전달받는걸로
        public Action RefreshParent { get; set; }
    }
}
