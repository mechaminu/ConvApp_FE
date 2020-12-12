using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserRegisterPage : ContentPage
    {
        public string OAuthId { get; private set; }
        public OAuthProvider OAuthType { get; private set; }
        public string Email { get; private set; }

        public UserRegisterPage(string id, OAuthProvider type, string email, Action action)
        {
            OAuthId = id;
            OAuthType = type;
            Email = email;
            Disappearing += (s,e) => action.Invoke();
            InitializeComponent();
        }
    }
}