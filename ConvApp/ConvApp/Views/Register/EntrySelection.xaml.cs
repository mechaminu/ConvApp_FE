﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntrySelection : ContentPage
    {
        public EntrySelection()
        {
            InitializeComponent();
        }

        public async void OnReview(object s, EventArgs e)
        {
            await Navigation.PushAsync(new ReviewEntry());
        }

        public async void OnRecipe(object s, EventArgs e)
        {
            await Navigation.PushAsync(new RecipeEntry());
        }
    }
}