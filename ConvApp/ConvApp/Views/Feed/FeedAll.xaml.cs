using ConvApp.Model;
using ConvApp.Views.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedAll : ContentPage
    {
      public static List<ImageSource> aa = new List<ImageSource>();
        public static ReviewPost reviewPostss = new ReviewPost();
        public FeedAll()
        {

            InitializeComponent();


            //LEFT.Children.Add(new ImageButton
            //{
            //    Source = ImageSource.FromUri(new Uri("https://ifh.cc/g/2MV3xE.jpg")),
            //    CornerRadius = 10,
            //    Aspect = Aspect.AspectFill,
            //    // Padding = new Thickness(0, 0, 0, 20)


            //aa.Add(ImageSource.FromUri(new Uri("https://ifh.cc/g/2MV3xE.jpg")));
            //    numfeed.Text = aa.Count().ToString();



            //foreach (ImageSource image in aa)
            //{
            //    LEFT.Children.Add(new ImageButton
            //    {
            //    Source = image,
            //    CornerRadius = 10,
            //     Aspect = Aspect.AspectFill,
            //    });
            //};

            



        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            refresh();
            numfeed.Text = aa.Count().ToString();
            int startnum = 0;
            foreach (ImageSource image in aa)
            {
                ImageButton imageButton = new ImageButton()
                {
                    Source = image,
                    CornerRadius = 10,
                    Aspect = Aspect.AspectFill,
                };
                imageButton.Clicked += OnImageButtonClicked;

                if (startnum % 2 == 0)
                {

                    LEFT.Children.Add(imageButton);
                }
                else if (startnum % 2 == 1)
                {
                    RIGHT.Children.Add(imageButton);
                }

                startnum++;
            };
            
        }
       async void OnImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowReview 
            { 
                BindingContext = reviewPostss 
            });
        }
        void refresh() 
        {
            LEFT.Children.Clear();
            RIGHT.Children.Clear();
        }
    }
}