using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ConvApp.ViewModels;
using System.Threading.Tasks;

namespace ConvApp.Views
{
    public partial class FeedPage : ContentPage
    {
        public static List<Post> posts = new List<Post>();
        public int currentPage = 0;

        public FeedPage()
        {
            InitializeComponent();
        }

        // 생성자가 아닌 OnAppearing 메소드에서 리스트뷰에 리스트를 제공해야 튕기지 않음!
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshList();
        }

        // ListView의 ItemsSource를 null로 만들었다 다시 할당하면 목록이 갱신됨!
        private void RefreshList()
        {
            list.ItemsSource = null;
            list.ItemsSource = posts;
        }

        async private Task<List<Post>> RefreshPage()
        {
            try
            {
                return await ApiManager.GetPostingPage(currentPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
                return null;
            }
        }

        async private void OnPostAdd(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostEntryPage());
            RefreshList();
        }

        async private void OnItemSelect(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new PostDetail
            {
                BindingContext = posts[e.ItemIndex]
            });
        }

        async private void OnRefreshPage(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.IsEnabled = false;
            posts = await RefreshPage();
            RefreshList();
            btn.IsEnabled = true;
        }

    }
}