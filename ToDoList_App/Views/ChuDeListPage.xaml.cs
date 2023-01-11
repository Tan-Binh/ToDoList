using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ToDoList_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ToDoList_App.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChuDeListPage
    {
        private async void LayChuDe()
        {
            HttpClient httpClient = new HttpClient();
            string result = await httpClient.GetStringAsync("http://" + App.ip + "/api/User/GetChuDeByUser?MaUser=" + App.UserId);
            List<ChuDe> ChuDe = JsonConvert.DeserializeObject<List<ChuDe>>(result);
            ChuDeList.ItemsSource = ChuDe;
        }

        public ChuDeListPage()
        {
            InitializeComponent();
            LayChuDe();
        }

        protected override void OnAppearing()
        {
            //Write the code of your page here
            LayChuDe();
            base.OnAppearing();
        }

        private void TICaNhan_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InfoUserPage());
        }

        private async void SWDelete_Invoked(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            ChuDe selectedChuDe = swipeItem.CommandParameter as ChuDe;
            bool answer = await DisplayAlert("Xóa chủ đề", "Bạn chắc chắn xóa?", "OK", "Hủy");
            if (answer == true)
            {
                ChuDe chuDe = new ChuDe
                {
                    MaChuDe = selectedChuDe.MaChuDe,
                };
                HttpClient http = new HttpClient();
                string jsonUser = JsonConvert.SerializeObject(chuDe);
                StringContent httcontent = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/DeleteChuDe", httcontent);
                string kqtv = await kq.Content.ReadAsStringAsync();
                if (kqtv != "0")
                {
                    LayChuDe();
                }
                else
                {
                    await DisplayAlert("Thông báo", "Đã có lỗi gì đó!", "OK");
                }
            }
        }

        private async void SWEdit_Invoked(object sender, EventArgs e)
        {
            SwipeItem swipeItem = sender as SwipeItem;
            ChuDe selectedChuDe = swipeItem.CommandParameter as ChuDe;
            string result = await DisplayPromptAsync("Sửa chủ đề", "Hãy nhập tên chủ đề mới", "OK", "Hủy");
            if (result != null)
            {
                ChuDe chuDe = new ChuDe
                {
                    MaChuDe = selectedChuDe.MaChuDe,
                    MaUser = selectedChuDe.MaUser,
                    TenChuDe = result
                };
                HttpClient http = new HttpClient();
                string jsonUser = JsonConvert.SerializeObject(chuDe);
                StringContent httcontent = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/UpdateChuDe", httcontent);
                string kqtv = await kq.Content.ReadAsStringAsync();
                if (kqtv != "0")
                {
                    LayChuDe();
                }
                else
                {
                    await DisplayAlert("Thông báo", "Chủ đề này đã tồn tại!", "OK");
                }
            }
        }

        private async void AddChuDeBtn_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Thêm chủ đề", "Hãy nhập tên chủ đề mới", "OK", "Hủy");
            if (result != null)
            {
                ChuDe chuDe = new ChuDe
                {
                    MaUser = App.UserId,
                    TenChuDe = result
                };
                HttpClient http = new HttpClient();
                string jsonChuDe = JsonConvert.SerializeObject(chuDe);
                StringContent httcontent = new StringContent(jsonChuDe, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/AddChuDe", httcontent);
                string kqtv = await kq.Content.ReadAsStringAsync();
                if (result != "" && kqtv != "0")
                {
                    LayChuDe();
                }
                else
                {
                    await DisplayAlert("Thông báo", "Chủ đề này đã tồn tại!", "OK");
                }
            }
        }

        private void ChuDeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChuDe selectedChuDe = (ChuDe)ChuDeList.SelectedItem;
            Navigation.PushAsync(new ViecLamPage(selectedChuDe));
        }
    }
}
