using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoList_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void CloseForgotPasswordPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void ResetPasswordBtn_Clicked(object sender, EventArgs e)
        {
            User user = new User
            {
                EmailUser = EmailEntry.Text
            };
            HttpClient http = new HttpClient();
            string jsonUser = JsonConvert.SerializeObject(user);
            StringContent httcontent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/CheckUserResetPassword", httcontent);
            var kqtv = await kq.Content.ReadAsStringAsync();
            if (kqtv != "0")
                await Navigation.PushAsync(new ResetPasswordPage(user));
            else
                await DisplayAlert("Thông báo", "Email này chưa được đăng ký!", "OK");
        }

        private void SinUpLabel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}