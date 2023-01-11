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
    public partial class InfoUserPage :ContentPage
    {
        User _user;

        async void LayUser()
        {
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync("http://" + App.ip + "/api/User/GetUser?MaUser=" + App.UserId);
            var user = JsonConvert.DeserializeObject<List<User>>(result);
            InfoUserList.BindingContext = user[0];
            _user = user[0];
        }
        public InfoUserPage()
        {
            InitializeComponent();
            LayUser();
        }

        private void ResetPasswordBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResetPasswordPage(_user));
        }

        private void LogOutBtn_Clicked(object sender, EventArgs e)
        {
            App.UserId = -1;
            Navigation.PushAsync(new LoginPage());
        }

        private async void SWEditTenUser_Invoked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Sửa tên", "Hãy nhập tên mới", "OK", "Hủy");
            if (result != null)
            {
                User user = new User
                {
                    MaUser = App.UserId,
                    TenUser = result
                };
                HttpClient http = new HttpClient();
                string jsonUser = JsonConvert.SerializeObject(user);
                StringContent httcontent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/UpdateTenUser", httcontent);
                var kqtv = await kq.Content.ReadAsStringAsync();
                if (kqtv != "0")
                {
                    LayUser();
                }
                else
                    await DisplayAlert("Thông báo", "Chân thành xin lỗi. Hệ thống đã gặp lỗi!", "OK");
            }
        }

        private async void SWEditEmailUser_Invoked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Sửa email", "Hãy nhập email mới", "OK", "Hủy");
            if (result != null)
            {
                User user = new User
                {
                    MaUser = App.UserId,
                    EmailUser = result
                };
                HttpClient http = new HttpClient();
                string jsonUser = JsonConvert.SerializeObject(user);
                StringContent httcontent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/UpdateEmailUser", httcontent);
                var kqtv = await kq.Content.ReadAsStringAsync();
                if (kqtv != "0")
                {
                    LayUser();
                }
                else
                    await DisplayAlert("Thông báo", "Email này đã tồn tại!", "OK");
            }
        }


    }
}