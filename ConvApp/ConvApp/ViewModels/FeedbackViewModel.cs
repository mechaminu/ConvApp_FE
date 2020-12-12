using ConvApp.Models;
using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class FeedbackViewModel : ViewModelBase
    {
        private bool isPopulated = false;

        private readonly byte type;
        private readonly long id;

        // 새 FeedbackViewModel이 생성된다는 의미는 해당 포스팅이 조회된다는 이야기이므로
        // 조회수 +1 메커니즘이 생성자에 같이 들어가야 할 듯
        public FeedbackViewModel(byte _type, long _id)
        {
            type = _type;
            id = _id;

            LikeBtnCommand = new Command(async () => await ToggleLike());
            RefreshCommand = new Command(async () => await Refresh());

            // Editor 뷰 요소를 파라미터로 전달받아 입력 Text 추출하여 댓글 업로드 시도하고, 성공적으로 업로드된 경우 입력 Text 공백으로 전환
            PostCmtCommand = new Command<List<View>>(async (list) => await PostComment(list));
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
                    OnChange(new PropertyChangedEventArgs(nameof(IsLiked)));
                    OnChange(new PropertyChangedEventArgs(nameof(LikeImage)));
                }
            }
        }

        public ImageSource LikeImage
        {
            get => IsLiked
                ? SvgImageSource.FromResource("ConvApp.Resources.heart-solid.svg")
                : SvgImageSource.FromResource("ConvApp.Resources.heart-regular.svg");
        }

        public async Task Refresh()
        {
            try
            {
                var (cmtList, likeList) = await ApiManager.GetFeedbacks(type, id);  // 튜플 받아오기. 한번의 Request.

                foreach (var cmt in cmtList)
                {
                    if (!(cmt.IsChild = type == (byte)FeedbackableType.Comment))
                    {
                        await cmt.Feedback.Refresh();
                        cmt.ChildCmtCommand = new Command(async () =>
                        {
                            if (await App.Current.MainPage.DisplayAlert("알림", "대댓글을 작성하시겠습니까?", "예", "아니오"))
                            {
                                //cmtEditor를 어떻게 받아올 수 있을까?
                                //cmtEditor.Placeholder = "활성화시 기본 표시문구가 변경됩니다";
                                //cmtEditor.Focus();

                                //void eh(object s, FocusEventArgs e)
                                //{
                                //    cmtEditor.Text = null;
                                //    cmtEditor.Placeholder = "취소했습니다!";

                                //    Task.Factory.StartNew(async () =>
                                //    {
                                //        await Task.Delay(1000);
                                //        MainThread.BeginInvokeOnMainThread(() => cmtEditor.Placeholder = "댓글 입력");
                                //    });

                                //    cmtEditor.Unfocused -= eh;
                                //}

                                //cmtEditor.Unfocused += eh;
                            }
                        });
                    }

                    cmt.RefreshParent = async () => await Refresh();
                    
                }

                isPopulated = false;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    comments.Clear();
                    foreach (var cmt in cmtList)
                        comments.Add(cmt);

                    likes.Clear();
                    foreach (var like in likeList)
                        likes.Add(like);
                });

                IsLiked = App.User != null && likeList.Exists(l => l.Creator.Id == App.User.Id);

                isPopulated = true;
            }
            catch (Exception ex)
            {
                await (App.Current.MainPage).DisplayAlert("오류", ex.Message, "확인");
            }
        }

        public async Task ToggleLike()
        {
            try
            {
                if (App.User == null)
                    throw new UnauthorizedAccessException("로그인 후 이용가능합니다!");

                if (isPopulated)
                    if (IsLiked)
                    {
                        await ApiManager.DeleteLike(type, id);
                        await Refresh();
                    }
                    else
                    {
                        await ApiManager.PostLike(type, id);
                        await Refresh();
                    }
                else
                {
                    await Refresh();
                    await ToggleLike();
                }
            }
            catch (UnauthorizedAccessException)
            {
                if (await App.Current.MainPage.DisplayAlert("", "로그인 후 이용가능한 메뉴입니다", "로그인으로 이동", "취소"))
                    await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                await (App.Current.MainPage).DisplayAlert("오류", ex.Message, "확인");
            }
        }

        public async Task PostComment(List<View> list)
        {
            try
            {
                if (App.User == null)
                    throw new UnauthorizedAccessException("로그인 후 이용가능합니다!");

                var cmtEditor = list[0] as Editor;
                var postBtn = list[1] as Button;

                postBtn.IsEnabled = false;

                if (cmtEditor.Text != null)
                {
                    await ApiManager.PostComment(type, id, (list[0] as Editor).Text);
                    await Refresh();
                }

                postBtn.IsEnabled = true;
                
                cmtEditor.Text = null;
                cmtEditor.Unfocus();
            } 
            catch (UnauthorizedAccessException)
            {
                if (await App.Current.MainPage.DisplayAlert("", "로그인 후 이용가능한 메뉴입니다", "로그인으로 이동", "취소"))
                    await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                await (App.Current.MainPage).DisplayAlert("오류", ex.Message, "확인");
            }
        }
    }
}
