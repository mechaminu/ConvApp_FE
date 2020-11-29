using System;
using System.Windows.Input;

using ConvApp.Models;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            this.DeleteComment = new Command(async () =>
            {
                await ApiManager.DeleteComment(this.Id);
                RefreshParent?.Invoke();
            });
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsModified { get; set; }

        public UserModel Creator { get; set; }
        public bool Owned { get => Creator.Id == App.User.Id; }
        public bool NotOwned { get => !Owned; }
        public string Text { get; set; }

        public FeedbackViewModel Feedback { get; set; }

        public ICommand DeleteComment { get; private set; }

        // 자식 요소인 댓글이 부모 요소인 FeedbackView의 새로고침을 Invoke 해야하는 상황
        // Dependency Injection을 쓰고 싶지만... 일단 delegate 전달받는걸로
        public Action RefreshParent { get; set; }
    }
}
