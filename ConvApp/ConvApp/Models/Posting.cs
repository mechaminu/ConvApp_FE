using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConvApp.Models;
using ConvApp.ViewModels;

namespace ConvApp.Models
{
    public class Posting
    {
        // 포스트 데이터베이스 저장형태 모델
        public int Id { get; set; }
        public bool IsRecipe { get; set; }
        public int CreatorId { get; set; }

        public DateTime Created { get; set; }
        public List<PostingNode> PostingNodes {get; set;}
        public List<Product> Products { get; set; }

        public async static Task<Post> ToPost(Posting posting)
        {
            if (posting.IsRecipe)
            {
                var titleNode = posting.PostingNodes[0];
                var textNode = posting.PostingNodes[1];

                var otherNodes = new List<PostContentNode>();
                foreach (var node in posting.PostingNodes.Skip(2))
                {
                    otherNodes.Add(new PostContentNode
                    {
                        NodeImage = Path.Combine(ApiManager.ImageEndPointURL,node.ImageFilename),
                        NodeString = node.Text
                    });
                }

                return new RecipePost
                {
                    User = await ApiManager.GetUserData(posting.CreatorId),
                    Date = posting.Created,
                    Title = titleNode.Text,
                    PostContent = textNode.Text,
                    RecipeNode = otherNodes
                };
            }
            else
            {
                var ratingNode = posting.PostingNodes[0];
                var textNode = posting.PostingNodes[1];
                var imgNode = posting.PostingNodes[2];

                return new ReviewPost
                {
                    User = await ApiManager.GetUserData(posting.CreatorId),
                    Date = posting.Created,
                    Rating = double.Parse(ratingNode.Text),
                    PostContent = textNode.Text,
                    PostImage = Path.Combine(ApiManager.ImageEndPointURL,imgNode.ImageFilename)
                };
            }
        }
    }

    public class PostingNode
    {
        public string Text { get; set; }
        public string ImageFilename { get; set; }
    }
}
