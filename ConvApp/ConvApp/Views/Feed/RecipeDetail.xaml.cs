using ConvApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeDetail : ContentPage
    {
        // TODO Dependency injection으로 FeedbackViewModel 받아올까 함
        public RecipeDetail()
        {
            InitializeComponent();

            BindingContextChanged += (s, e) => ShowNodes();
        }

        private void ShowNodes()
        {
            if (BindingContext == null)
                return;

            var btx = BindingContext as RecipeViewModel;

            nodeArea.Children.Clear();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var node in btx.RecipeNode)
                    nodeArea.Children.Add(new RecipeNodeCell { BindingContext = node });
            });
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
            try
            {
                if (cmtEditor.Text != null)
                    await (BindingContext as PostingViewModel).Feedback.PostComment(cmtEditor.Text);
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "확인");
                return;
            }

            cmtEditor.Text = null;
            cmtEditor.Unfocus();
        }
    }
}