using ConvApp.Models;
using ConvApp.Models.Auth;
using ConvApp.ViewModels;
using FFImageLoading;
using HeyRed.Mime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserRegisterPage : ContentPage
    {
        public UserRegisterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var bc = BindingContext as UserRegisterViewModel;
            oauthLabel.IsVisible = bc.OAuthType != OAuthProvider.None;
            pwdEntry.IsVisible = bc.OAuthType == OAuthProvider.None;
            pwd2Entry.IsVisible = bc.OAuthType == OAuthProvider.None;
            emailLabel.IsVisible = bc.Email != null;
            emailEntry.IsVisible = !(bc.Email != null);

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (pwdEntry.Text != pwd2Entry.Text)
            {
                await App.Current.MainPage.DisplayAlert("오류", "비밀번호 확인 입력이 일치하지 않습니다.", "확인");
                return;
            }

            var bc = BindingContext as UserRegisterViewModel;

            

            var dto = bc.GetDTO();
            dto.Email = dto.Email == null ? emailEntry.Text : dto.Email;
            dto.Name = nameEntry.Text;
            dto.Password = pwdEntry.Text;

            // 이미지 업로드
            dto.Image = await ApiManager.UploadImage(resultList);

            try
            {
                App.User = await ApiManager.RegisterUser(dto);

                Preferences.Set("auth", JsonConvert.SerializeObject(new AuthContext { LastLogined = DateTime.UtcNow, Type = (OAuthProvider)dto.OAuthProvider }));
                bc.isSuccessful = true;
                App.Current.MainPage = new AppShell();
            }
            catch (Exception ex) when (ex.Message == "동일 이메일로 가입정보가 이미 존재합니다")
            {
                if (dto.OAuthProvider == (byte)OAuthProvider.None)
                    await App.Current.MainPage.DisplayAlert("오류", ex.Message, "확인");
                else
                    if (await App.Current.MainPage.DisplayAlert("알림", $"SNS가 사용중인 이메일로 가입정보가 존재합니다.\n{dto.Email}\n 해당 계정에 SNS 로그인 정보를 추가할까요?", "예", "아니오"))
                        App.User = await ApiManager.UpdateOAuth(dto);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("오류", ex.Message, "확인");
            }
        }

        Stream imgStream;
        private List<FileResult> resultList = new List<FileResult>();

        private void AddImage(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var pickResult = await FilePicker.PickAsync(new PickOptions { PickerTitle = "사진 선택", FileTypes = FilePickerFileType.Images });
                if (pickResult != null)
                {
                    pickResult.ContentType = MimeTypesMap.GetMimeType(pickResult.FileName);

                    imgStream = await pickResult.OpenReadAsync();
                    profileImg.Source = ImageSource.FromStream(() => imgStream);

                    List<FileResult> fileResults = new List<FileResult>();
                    fileResults.Add(pickResult);
                    resultList = fileResults;
                }
                else
                    return;
            });
        }
    }
}