using ConvApp.ViewModels;
using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class CommentModel
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte ParentType { get; set; }
        public long ParentId { get; set; }
        public long UserId { get; set; }
        public string Text { get; set; }

        public static async Task<CommentViewModel> Populate(CommentModel model)
        {
            return new CommentViewModel
            {
                Id = model.Id,
                Date = model.CreatedDate.ToLocalTime(),
                Creator = await ApiManager.GetUser(model.UserId),
                Text = model.Text,
                Feedback = new FeedbackViewModel(2, model.Id)
            };
        }
    }
}
