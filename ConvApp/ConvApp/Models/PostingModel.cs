﻿using ConvApp.ViewModels;
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

    public class PostingModel
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public byte PostingType { get; set; }
        public long UserId { get; set; }

        public List<ProductModel> Products { get; set; }
        public List<PostingNodeModel> PostingNodes { get; set; }

        public string ProfileImage { get; private set; }

        public static async Task<PostingViewModel> Populate(PostingModel model)
        {
            var user = await ApiManager.GetUser(model.UserId);

            switch (model.PostingType)
            {
                case (byte)PostingTypes.RECIPE:

                    var otherNodes = new List<PostingNode>();
                    foreach (var node in model.PostingNodes.Skip(2))
                    {
                        otherNodes.Add(new PostingNode
                        {
                            NodeImage = node.Image,
                            NodeString = node.Text
                        });
                    }

                    return new RecipeViewModel
                    {
                        Id = model.Id,
                        User = user,
                        Date = model.ModifiedDate.ToLocalTime(),
                        Title = model.PostingNodes[0].Text,
                        PostContent = model.PostingNodes[1].Text,
                        Products = model.Products,
                        RecipeNode = otherNodes
                    };

                default:
                    var tmp = new ReviewViewModel
                    {
                        Id = model.Id,
                        User = user,
                        Date = model.ModifiedDate.ToLocalTime(),
                        Products = model.Products,
                        Rating = double.Parse(model.PostingNodes[0].Text),
                        PostContent = model.PostingNodes[1].Text,
                        PostImage = model.PostingNodes[2].Image
                    };
                    return tmp;
            }
        }
    }

    public class PostingNodeModel
    {
        public string Text { get; set; }
        public string ImageFilename { get; set; }
        public string Image
        {
            get => ImageFilename != null
                ? Path.Combine(ApiManager.ImageEndPointURL, ImageFilename)
                : string.Empty;
        }
    }
}
