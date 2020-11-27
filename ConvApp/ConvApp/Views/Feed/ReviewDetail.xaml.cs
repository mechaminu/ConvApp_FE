using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConvApp.ViewModels;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewDetail : ContentPage
    {
        // TODO Dependency injection으로 FeedbackViewModel 받아올까 함
        public ReviewDetail()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // TODO 댓글 ListView 고치는 스크립트
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            cmtEditor.Placeholder = "활성화시 기본 표시문구가 변경됩니다";
            cmtEditor.Focus();

            void eh(object s, FocusEventArgs e)
            {
                cmtEditor.Text = null;
                cmtEditor.Placeholder = "취소했습니다!";

                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(1000);
                    MainThread.BeginInvokeOnMainThread(() => cmtEditor.Placeholder = "댓글 입력");
                });

                cmtEditor.Unfocused -= eh;
            }

            cmtEditor.Unfocused += eh;
        }

        private async void PostBtnClicked(object sender, EventArgs e)
        {
            if (cmtEditor.Text != null)
                await (BindingContext as PostingViewModel).Feedback.PostComment(cmtEditor.Text);

            cmtEditor.Text = null;
            cmtEditor.Unfocus();
        }
    }
}