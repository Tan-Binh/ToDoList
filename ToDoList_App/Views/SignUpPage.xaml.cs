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
    public partial class SignUpPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpPage" /> class.
        /// </summary>
        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private void SinInLabel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        private async void SignUpBtn_Clicked(object sender, EventArgs e)
        {
            User user = new User
            {
                TenUser = NameEntry.Text,
                EmailUser = EmailEntry.Text,
                PasswordUser = PasswordEntry.Text
            };
            if (PasswordEntry.Text == ConfirmPasswordEntry.Text)
            {
                HttpClient http = new HttpClient();
                string jsonUser = JsonConvert.SerializeObject(user);
                StringContent httcontent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/AddUser", httcontent);
                var kqtv = await kq.Content.ReadAsStringAsync();
                if (kqtv != "0")
                {
                    await DisplayAlert("Thông báo", "Bạn đăng ký thành công!", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Thông báo", "Email này đã được sử dụng!", "OK");
                }
            }
            else
                await DisplayAlert("Thông báo", "Mật khẩu chưa trùng khớp!", "OK");
        }

        private void CloseSinUpPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}