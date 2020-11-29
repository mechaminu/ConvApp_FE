using ConvApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackView : StackLayout
    {
        bool cmtPopulated = false;

        public FeedbackView()
        {
            InitializeComponent();

            BindingContextChanged += (s, e) => ShowComments();
        }

        private void ShowComments()
        {
            if (cmtPopulated || BindingContext == null)
                return;

            var btx = BindingContext as FeedbackViewModel;

            commentArea.Children.Clear();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var cmt in btx.Comments)
                    commentArea.Children.Add(new CommentCell { BindingContext = cmt });
            });

            btx.Comments.CollectionChanged += (s, e) =>
            {
                cmtPopulated = false;
                ShowComments();
            };

            cmtPopulated = true;
        }
    }
}