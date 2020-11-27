using System;
using System.Threading.Tasks;
using ConvApp.ViewModels;

namespace ConvApp.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Text { get; set; }

        public static async Task<CommentViewModel> Populate(CommentModel model)
        {
            return new CommentViewModel
            {
                Id = model.Id,
                Date = model.ModifiedDate.ToLocalTime(),
                IsModified = model.CreatedDate != model.ModifiedDate,
                Creator = await ApiManager.GetUser(model.CreatorId),
                Text = model.Text,
                Feedback = new FeedbackViewModel(2, model.Id)
            };
        }
    }
}
