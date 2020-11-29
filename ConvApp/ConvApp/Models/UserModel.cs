using System.IO;

namespace ConvApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageFilename { get; set; }

        public string Image
        {
            get => ImageFilename != null
                ? Path.Combine(ApiManager.ImageEndPointURL, ImageFilename)
                : string.Empty;
        }
    }
}