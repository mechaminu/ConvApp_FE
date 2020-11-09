using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConvApp.ViewModels;

namespace ConvApp.Models
{
    public class Posting
    {
        // 포스트 데이터베이스 저장형태 모델
        public long id { get; set; }
        public DateTime create_date { get; set; }
        public DateTime modify_date { get; set; }
        public long create_user_oid { get; set; }

        public byte pst_type { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string images { get; set; }
        public string products { get; set; }

        public async static Task<Post> ToPost(Posting posting)
        {
            var tmpList = new List<byte[]>();

            foreach (var e in posting.images.Split(','))
            {
                tmpList.Add(Encoding.UTF8.GetBytes(await new StreamReader(await ApiManager.GetImage(e)).ReadToEndAsync()));
            }

            return new Post(posting.id, posting.create_date, posting.modify_date, "" + posting.create_user_oid, null)
            {
                Type = (PostType)posting.pst_type,
                PostTitle = posting.title,
                PostContent = posting.text,
                PostImage = tmpList
            };
        }
    }
}
