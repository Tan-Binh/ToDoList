using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoList_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ToDoList_App.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordPage" /> class.
        /// </summary>
        User _user;
        public ResetPasswordPage()
        {
            this.InitializeComponent();
        }

        public ResetPasswordPage(User user)
        {
            this.InitializeComponent();
            _user = user;
        }

        private void CloseResetPasswordPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void SinUpLabel_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private async void SubmitBtn_Clicked(object sender, System.EventArgs e)
        {
            User user = new User
            {
                EmailUser = _user.EmailUser,
                PasswordUser = NewPasswordEntry.Text
            };
            HttpClient http = new HttpClient();
            string jsonUser = JsonConvert.SerializeObject(user);
            StringContent httcontent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/UpdatePassword", httcontent);
            var kqtv = await kq.Content.ReadAsStringAsync();
            if (NewPasswordEntry.Text == ConfirmNewPasswordEntry.Text)
            {
                if (kqtv != "0")
                {
                    await DisplayAlert("Thông báo", "Bạn đã đổi mật khẩu thành công!", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                    await DisplayAlert("Thông báo", "Chân thành xin lỗi. Hệ thống đã gặp lỗi!", "OK");
            }
            else
                await DisplayAlert("Thông báo", "Mật khẩu chưa trùng nhau!", "OK");
        }
    }
}