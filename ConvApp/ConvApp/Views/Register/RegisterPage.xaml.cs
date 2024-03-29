﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void PostReview(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReviewEntry());
        }

        private async void PostRecipe(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecipeEntry());
        }
    }
}