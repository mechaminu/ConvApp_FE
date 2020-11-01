using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class Post
    {
        // 포스트 전시에 사용하는 뷰모델
        public Post(long id, DateTime createdate, DateTime modifydate, string un, Stream ui)
        {
            Id = id;
            CreateDate = createdate;
            ModifyDate = modifydate;
            UserName = un;
            UserImage = ui;
        }

        // DB 입력시 결정되는 값
        public long Id { get; private set; }    
        public DateTime CreateDate { get; private set; }
        public DateTime ModifyDate { get; private set; }
        public string UserName { get; private set;  }
        public Stream UserImage { get; private set; }

        // 유저 입력으로 결정되는 값
        public PostType Type { get; set; }
        public string PostTitle { get; set; }       // 25자 미만일 것
        public string PostContent { get; set; }     // 900자 미만일 것

        public List<byte[]> PostImage { get; set; }
        
        // TODO
        // 상품목록, 별점, 조회수
        // 좋아요, 댓글, 신고 등 피드백 연계
    }

    public enum PostType : byte
    {
       UserRecipe,  // 유저레시피, 0
       ProdReview   // 상품평가, 1
    }
}
