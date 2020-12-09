using ConvApp.Models;
using System;
using System.Collections.ObjectModel;

namespace ConvApp.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string StoreName { get => ProductModel.StoreNames[this.StoreId]; }
        public string CategoryName { get => ProductModel.CategoryNames[this.CategoryId]; }

        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }

        public string Calory { get; set; }  // 상품의 칼로리 정보
        public string Rank { get; set; }    // 상품의 랭킹
        public string Rate { get; set; }    // 상품의 평점 

        public ObservableCollection<ReviewViewModel> ReviewList { get; set; } // 상품관련 상품평 리스트 
        public ObservableCollection<RecipeViewModel> RecipeList { get; set; } // 상품관련 레시피 리스트 
    }
}
