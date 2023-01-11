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
    public partial class LoginPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void ForgotPasswordLabel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async void SignInBtn_Clicked(object sender, EventArgs e)
        {
            User user = new User { EmailUser = EmailEntry.Text, PasswordUser = PasswordEntry.Text };
            HttpClient http = new HttpClient();
            string jsonUser = JsonConvert.SerializeObject(user);
            StringContent httcontent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/CheckUserLogin", httcontent);
            var kqtv = await kq.Content.ReadAsStringAsync();
            if (kqtv != "0")
            {
                App.UserId = int.Parse(kqtv);
                await Navigation.PushAsync(new ChuDeListPage());
            }
            else
            {
                await DisplayAlert("Thông báo", "Bạn chưa nhập đúng email hoặc mật khẩu!", "OK");
            }
        }

        private void SignUpLabel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}