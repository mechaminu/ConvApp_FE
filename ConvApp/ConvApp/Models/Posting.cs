using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConvApp.ViewModels;

namespace ConvApp.Models
{
    public enum PostingTypes : byte
    {
        REVIEW,
        RECIPE
    }

    public class Posting
    {
        // 포스트 데이터베이스 저장형태 모델
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public byte PostingType { get; set; }
        public int CreatorId { get; set; }

        public List<Product> Products { get; set; }
        public List<PostingNode> PostingNodes {get; set;}

        public async static Task<Post> ToPost(Posting posting)
        {
            switch (posting.PostingType)
            {
                case (byte)PostingTypes.RECIPE:

                    var otherNodes = new List<PostNode>();

                    foreach (var node in posting.PostingNodes.Skip(2))
                    {
                        otherNodes.Add(new PostNode
                        {
                            NodeImage = node.Image != null ? Path.Combine(ApiManager.ImageEndPointURL,node.Image) : string.Empty,
                            NodeString = node.Text
                        });
                    }

                    return new RecipePost
                    {
                        User = await ApiManager.GetUserData(posting.CreatorId),
                        Date = posting.ModifiedDate,
                        Title = posting.PostingNodes[0].Text,
                        PostContent = posting.PostingNodes[1].Text,
                        RecipeNode = otherNodes
                    };

                //case (byte)PostingTypes.REVIEW:
                default:

                    return new ReviewPost
                    {
                        User = await ApiManager.GetUserData(posting.CreatorId),
                        Date = posting.ModifiedDate,
                        Rating = double.Parse(posting.PostingNodes[0].Text),
                        PostContent = posting.PostingNodes[1].Text,
                        PostImage = posting.PostingNodes[2].Image != null ? Path.Combine(ApiManager.ImageEndPointURL, posting.PostingNodes[2].Image) : string.Empty
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
