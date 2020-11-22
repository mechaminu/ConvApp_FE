using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConvApp.ViewModels
{
    public class FeedbackViewModel
    {
        private byte type;
        private int id;

        public FeedbackViewModel(byte _type, int _id)
        {
            type = _type;
            id = _id;

            Refresh();
        }

        private ObservableCollection<Comment> comments;
        public ObservableCollection<Comment> Comments
        {
            get => comments;
        }

        private ObservableCollection<Like> likes;
        public ObservableCollection<Like> Likes
        {
            get => likes;
        }

        private List<Comment> commentList;
        private List<Like> likeList;

        public async Task Refresh()
        {
            await Task.Factory.StartNew(() =>
            {
                Task.WaitAll(
                    new Task(async () => commentList = await ApiManager.GetComments(type, id, DateTime.UtcNow, 0)),
                    new Task(async () => likeList = await ApiManager.GetLikes(type, id)));
            });

            comments = new ObservableCollection<Comment>(commentList);
            likes = new ObservableCollection<Like>(likeList);
        }
    }
}
