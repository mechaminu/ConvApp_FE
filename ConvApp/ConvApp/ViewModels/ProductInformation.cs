using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.ViewModels
{
    public class ProductInformation
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
    
        public string Calory { get; set; }  //  상품의 칼로리 정보
        public string Rank { get; set; } // 상품의 랭킹
        public string Rate { get; set; } // 상품의 평점 

        public List<ReviewPost> Reviewpostlist { get; set; } // 상품에 대한 관련 후기 리스트 
    }
}
