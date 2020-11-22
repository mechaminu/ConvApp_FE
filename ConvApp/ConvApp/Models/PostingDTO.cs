using ConvApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public enum PostingTypes : byte
    {
        REVIEW,
        RECIPE
    }

    public class PostingDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public byte PostingType { get; set; }
        public int CreatorId { get; set; }

        public List<ProductDTO> Products { get; set; }
        public List<PostingNode> PostingNodes { get; set; }

        public async static Task<PostingDetailViewModel> PopulateDTO(PostingDTO posting)
        {
            switch (posting.PostingType)
            {
                case (byte)PostingTypes.RECIPE:

                    var otherNodes = new List<PostNode>();

                    foreach (var node in posting.PostingNodes.Skip(2))
                    {
                        otherNodes.Add(new PostNode
                        {
                            NodeImage = node.Image != null ? Path.Combine(ApiManager.ImageEndPointURL, node.Image) : string.Empty,
                            NodeString = node.Text
                        });
                    }

                    return new RecipePost
                    {
                        User = await ApiManager.GetUserData(posting.CreatorId),
                        Date = posting.ModifiedDate,
                        Title = posting.PostingNodes[0].Text,
                        PostContent = posting.PostingNodes[1].Text,
                        RecipeNode = otherNodes,
                        Feedback = await ApiManager.GetFeedback(0, posting.Id)
                    };

                //case (byte)PostingTypes.REVIEW:
                default:

                    return new ReviewPost
                    {
                        User = await ApiManager.GetUserData(posting.CreatorId),
                        Date = posting.ModifiedDate,
                        Rating = double.Parse(posting.PostingNodes[0].Text),
                        PostContent = posting.PostingNodes[1].Text,
                        PostImage = posting.PostingNodes[2].Image != null ? Path.Combine(ApiManager.ImageEndPointURL, posting.PostingNodes[2].Image) : string.Empty,
                        Feedback = await ApiManager.GetFeedback(0, posting.Id)
                    };
            }
        }
    }

    public class PostingNode
    {
        public string Text { get; set; }
        public string Image { get; set; }
    }
}
