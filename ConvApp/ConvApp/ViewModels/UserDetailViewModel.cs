using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConvApp.ViewModels
{
    public class UserDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public List<PostingViewModel> Postings { get; set; }
        public List<PostingViewModel> LikedPostings { get; set; }
        public List<ProductViewModel> LikedProducts { get; set; }
        public List<UserBriefModel> FollowingUsers { get; set; }
        public List<UserBriefModel> FollowerUsers { get; set; }
    }
}
