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

        public async Task<PostingDetailViewModel> PopulateDTO()
        {
            switch (this.PostingType)
            {
                case (byte)PostingTypes.RECIPE:

                    var otherNodes = new List<PostNode>();

                    foreach (var node in this.PostingNodes.Skip(2))
                    {
                        otherNodes.Add(new PostNode
                        {
                            NodeImage = node.Image != null ? Path.Combine(ApiManager.ImageEndPointURL, node.Image) : string.Empty,
                            NodeString = node.Text
                        });
                    }

                    return new RecipePostingViewModel
                    {
                        Id = this.Id,
                        User = await ApiManager.GetUserData(this.CreatorId),
                        Date = this.ModifiedDate,
                        Title = this.PostingNodes[0].Text,
                        PostContent = this.PostingNodes[1].Text,
                        Products = this.Products,
                        RecipeNode = otherNodes
                    };

                //case (byte)PostingTypes.REVIEW:
                default:
                    return new ReviewPostingViewModel
                    {
                        Id = this.Id,
                        User = await ApiManager.GetUserData(this.CreatorId),
                        Date = this.ModifiedDate,
                        Products = this.Products,
                        Rating = double.Parse(this.PostingNodes[0].Text),
                        PostContent = this.PostingNodes[1].Text,
                        PostImage = this.PostingNodes[2].Image != null ? Path.Combine(ApiManager.ImageEndPointURL, this.PostingNodes[2].Image) : string.Empty
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
