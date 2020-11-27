using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ConvApp.Models;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class CommentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsModified { get; set; }

        public UserModel Creator { get; set; }
        public bool Owned { get => Creator.Id == App.User.Id; }
        public string Text { get; set; }

        public FeedbackViewModel Feedback { get; set; }
    }
}
