﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ConvApp.Model;

namespace ConvApp.Views
{
    public partial class FeedPage : TabbedPage
    {
        //public static List<Post> posts = new List<Post>();
        public static List<ReviewPost> reviewPosts = new List<ReviewPost>();
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
           // list.ItemsSource = posts;
            list.ItemsSource = reviewPosts;

            list2.ItemsSource = null;
            // list.ItemsSource = posts;
            list2.ItemsSource = reviewPosts;
            
        }

        async private void OnItemSelect(object sender, ItemTappedEventArgs e)
        {/*
            await Navigation.PushAsync(new PostDetail
            {
                //BindingContext = posts[e.ItemIndex]
                BindingContext = reviewPosts[e.ItemIndex]
            });*/
        }

        private void OnRefresh(object sender, EventArgs e)
        {
                var list = (ListView)sender;
                //put your refreshing logic here
                var itemList = reviewPosts;
                reviewPosts.Clear();
                foreach (var s in itemList)
                {
                    reviewPosts.Add(s);
                }
                //make sure to end the refresh state
                list.IsRefreshing = false;
            
        }
    }
}