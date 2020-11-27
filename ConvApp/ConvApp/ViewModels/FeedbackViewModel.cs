using ConvApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        private readonly byte type;
        private readonly int id;
        private bool isPopulated = false;

        // 새 FeedbackViewModel이 생성된다는 의미는 해당 포스팅이 조회된다는 이야기이므로
        // 조회수 +1 메커니즘이 생성자에 같이 들어가야 할 듯
        public FeedbackViewModel(byte _type, int _id)
        {
            type = _type;
            id = _id;

            // TODO 조회수 +1 << IHasViewCount 대상에 한해서만!


            LikeBtnCommand = new Command(async () => await ToggleLike());
            RefreshCommand = new Command(async () => await Refresh());

            // Editor 뷰 요소를 파라미터로 전달받아 입력 Text 추출하여 댓글 업로드 시도하고, 성공적으로 업로드된 경우 입력 Text 공백으로 전환
            PostCmtCommand = new Command<string>(async (t) => await PostComment(t));
        }

        public ICommand LikeBtnCommand { protected set; get; }
        public ICommand RefreshCommand { protected set; get; }
        public ICommand PostCmtCommand { protected set; get; }

        private ObservableCollection<CommentViewModel> comments = new ObservableCollection<CommentViewModel>();
        public ObservableCollection<CommentViewModel> Comments
        {
            get => comments;
        }

        private ObservableCollection<Like> likes = new ObservableCollection<Like>();
        public ObservableCollection<Like> Likes
        {
            get => likes;
        }

        private bool isLiked;
        public bool IsLiked
        {
            get => isLiked;

            set
            {
                if (value != isLiked)
                {
                    isLiked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLiked)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LikeImage)));
                }
            }
        }

        public ImageSource LikeImage
        {
            get => IsLiked
                ? ImageSource.FromResource("ConvApp.Resources.heartfilled.png")
                : ImageSource.FromResource("ConvApp.Resources.heart.png");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Refresh()
        {
            try
            {
                var cmtList = await ApiManager.GetComments(type, id);
                var likeList = await ApiManager.GetLikes(type, id);

                foreach (var cmt in cmtList)
                {
                    await cmt.Feedback.Refresh();
                }

                isPopulated = false;
                comments.Clear();


                comments = new ObservableCollection<CommentViewModel>(cmtList);
                likes.Clear();
                likes = new ObservableCollection<Like>(likeList);
                IsLiked = likeList.Exists(l => l.Creator.Id == App.User.Id);

                isPopulated = true;
            }
            catch
            {
                throw new Exception("feedback refresh faild. try again");
            }
        }

        public async Task ToggleLike()
        {
            if (isPopulated)
                if (IsLiked)
                {
                    try
                    {
                        var likeList = await ApiManager.DeleteLike(type, id);

                        likes.Clear();
                        likeList.ForEach(l => likes.Add(l));
                        IsLiked = false;
                    }
                    catch
                    {
                        throw new Exception("like deletion failed");
                    }
                }
                else
                {
                    try
                    {
                        var likeList = await ApiManager.PostLike(type, id);
                        likes.Clear();
                        likeList.ForEach(l => likes.Add(l));
                        IsLiked = true;
                    }
                    catch
                    {
                        throw new Exception("like creation failed");
                    }
                }
            else
                throw new Exception("wait for viewmodel to be populated");
        }

        public async Task PostComment(string text)
        {
            var cmt = await ApiManager.PostComment(type, id, text);
            comments.Add(cmt);
        }
    }
}
