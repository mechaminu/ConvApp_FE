using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConvApp.ViewModels;
using Xamarin.Forms.Internals;
using System.Linq;
using FFImageLoading;
using FFImageLoading.Work;
using Xamarin.Forms;
using FFImageLoading.Forms;
using ImageSource = Xamarin.Forms.ImageSource;

namespace ConvApp.Models
{
    public class Posting
    {
        // 포스트 데이터베이스 저장형태 모델
        public long id { get; set; }
        public long create_user_oid { get; set; }
        public DateTime create_date { get; set; }
        public DateTime modify_date { get; set; }
        public DateTime delete_date { get; set; }

        public bool is_recipe { get; set; }
        public List<PostingNodeClient> contentNodes {get; set;}
        public List<long> products {get; set;}

        public async static Task<Post> ToPost(Posting posting)
        {
            var creator = await ApiManager.GetUserData(posting.create_user_oid);

            if (posting.is_recipe)
            {
                var tmpDate = posting.modify_date == null ? posting.create_date : posting.modify_date;

                var titleNode = posting.contentNodes[0];
                var textNode = posting.contentNodes[1];

                var otherNodes = new List<PostContentNode>();
                foreach (var i in posting.contentNodes.Skip(2))
                {
                    foreach(var filename in i.image.Split(';'))
                    {
                        var img = (await ApiManager.GetImage(filename)).ToByteArray();

                        otherNodes.Add(new PostContentNode
                        {
                            NodeImage = ImageSource.FromStream(() => new MemoryStream(img)),
                            NodeString = i.text
                        });
                    }
                }

                return new RecipePost
                {
                    User = creator,
                    Date = tmpDate,
                    IsModified = posting.modify_date == null,
                    Title = titleNode.text,
                    PostContent = textNode.text,
                    RecipeNode = otherNodes
                };
            } else
            {
                return new ReviewPost
                {

                };
            }
        }
    }

    public class PostingNodeClient
    {
        public string image { get; set; }
        public string text { get; set; }
    }
}
