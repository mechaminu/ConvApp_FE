using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class FeedbackViewModel : INotifyPropertyChanged
    {
        private byte type;
        private int id;
        private bool isPopulated = false;

        public FeedbackViewModel(byte _type, int _id)
        {
            type = _type;
            id = _id;
            LikeBtnCommand = new Command(async () => await ToggleLike());
            RefreshCommand = new Command(async () => await Refresh());
        }

        public ICommand LikeBtnCommand { protected set; get; }
        public ICommand RefreshCommand { protected set; get; }

        private ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
        public ObservableCollection<Comment> Comments
        {
            get => comments;
        }

        private ObservableCollection<Like> likes = new ObservableCollection<Like>();
        public ObservableCollection<Like> Likes
        {
            get => likes;
        }

        private bool isLiked;
        public bool IsLiked
        {
            get => isLiked;

            set
            {
                if (value != isLiked)
                {
                    isLiked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLiked"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Refresh()
        {
            try
            {
                var cmtList = await ApiManager.GetComments(type, id, DateTime.UtcNow, 0);
                var likeList = await ApiManager.GetLikes(type, id);

                isPopulated = false;
                comments.Clear();
                comments = new ObservableCollection<Comment>();
                likes.Clear();
                likes = new ObservableCollection<Like>(likeList);
                IsLiked = likeList.Exists(l => l.Creator.Id == App.User.Id);

                isPopulated = true;
            }
            catch
            {
                throw new Exception("feedback refresh faild. try again");
            }
        }

        public async Task ToggleLike()
        {
            if (isPopulated)
                if (IsLiked)
                {
                    try
                    {
                        var likeList = await ApiManager.DeleteLike(type, id);

                        likes.Clear();
                        likeList.ForEach(l => likes.Add(l));
                        IsLiked = false;
                    }
                    catch
                    {
                        throw new Exception("like deletion failed");
                    }
                }
                else
                {
                    try
                    {
                        var likeList = await ApiManager.PostLike(type, id);
                        likes.Clear();
                        likeList.ForEach(l => likes.Add(l));
                        IsLiked = true;
                    }
                    catch
                    {
                        throw new Exception("like creation failed");
                    }
                }
            else
                throw new Exception("wait for viewmodel to be populated");
        }
    }
}
