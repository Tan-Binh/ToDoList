using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoList_App.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList_App.Views
{
    public partial class ViecLamPage
    {
        private static ChuDe _chuDe;
        private ViecLam _viecLam;
        private static bool isAdd;
        async void LayViecLam()
        {
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync("http://" + App.ip + "/api/User/GetViecLamByChuDe?MaChuDe=" + _chuDe.MaChuDe);
            var viecLam = JsonConvert.DeserializeObject<List<ViecLam>>(result);
            foreach(ViecLam item in viecLam)
            {
                if (item.ThoiGian != null)
                {
                    DateTime itemDate = DateTime.ParseExact(item.ThoiGian, "yyyy-MM-ddTHH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture);
                    item.ThoiGian = itemDate.ToString("dd-MM-yyyy HH:mm:ss");
                }
                else
                    item.ThoiGian = "";
            }
            ViecLamList.ItemsSource = viecLam;
        }

        public ViecLamPage()
        {
            this.InitializeComponent();
        }

        public ViecLamPage(ChuDe selectedChuDe)
        {
            InitializeComponent();
            Title = selectedChuDe.TenChuDe;
            _chuDe = selectedChuDe;
            LayViecLam();
        }

        private void TICaNhan_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new InfoUserPage());
        }

        private async void SWDelete_Invoked(object sender, System.EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var selectedViecLam = swipeItem.CommandParameter as ViecLam;
            bool answer = await DisplayAlert("Xóa việc làm", "Bạn chắc chắn xóa?", "OK", "Hủy");
            if (answer == true)
            {
                ViecLam viecLam = new ViecLam
                {
                    MaViecLam = selectedViecLam.MaViecLam,
                };
                HttpClient http = new HttpClient();
                string jsonViecLam = JsonConvert.SerializeObject(viecLam);
                StringContent httcontent = new StringContent(jsonViecLam, Encoding.UTF8, "application/json");
                HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/DeleteViecLam", httcontent);
                var kqtv = await kq.Content.ReadAsStringAsync();
                if (kqtv != "0")
                {
                    LayViecLam();
                }
                else
                    await DisplayAlert("Thông báo", "Đã có lỗi gì đó!", "OK");
            }
        }

        private void SWEdit_Invoked(object sender, System.EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var selectedViecLam = swipeItem.CommandParameter as ViecLam;
            isAdd = false;
            _viecLam = selectedViecLam;
            ViecLamLabelPopup.Text = "Sửa việc làm";
            ViecLamEntry.Text = selectedViecLam.NoiDungViecLam;
            DateTimeEntry._entry.Text = selectedViecLam.ThoiGian;
            AddViecLamPopup.Text = "Sửa";
            popupAddView.IsVisible = true;
        }

        private void AddViecLamBtn_Clicked(object sender, EventArgs e)
        {
            isAdd = true;
            ViecLamLabelPopup.Text = "Thêm việc làm";
            ViecLamEntry.Text = "";
            DateTimeEntry._entry.Text = "";
            AddViecLamPopup.Text = "Thêm";
            popupAddView.IsVisible = true;
        }

        private async void AddEditViecLam(ViecLam viecLam, bool isAdd)
        {
            HttpClient http = new HttpClient();
            string jsonViecLam = JsonConvert.SerializeObject(viecLam);
            StringContent httcontent = new StringContent(jsonViecLam, Encoding.UTF8, "application/json");
            HttpResponseMessage kq;
            if (isAdd == true)
            {
                kq = await http.PostAsync("http://" + App.ip + "/api/User/AddViecLam", httcontent);
            }
            else
            {
                kq = await http.PostAsync("http://" + App.ip + "/api/User/UpdateViecLam", httcontent);
            }
            string kqtv = await kq.Content.ReadAsStringAsync();
            if (viecLam.NoiDungViecLam != "" && kqtv != "0")
            {
                popupAddView.IsVisible = false;
                LayViecLam();
            }
            else
                await DisplayAlert("Thông báo", "Việc làm này đã tồn tại!", "OK");
        }

        private void AddViecLamPopup_Clicked(object sender, EventArgs e)
        {
            DateTime myDate;
            string timeEntry = "";
            string dateTimePicker = "" + DateTimeEntry._entry.Text;
            if (dateTimePicker != "")
            {
                myDate = DateTime.ParseExact(dateTimePicker, "dd-MM-yyyy HH:mm:ss",
                                           System.Globalization.CultureInfo.InvariantCulture);
                timeEntry += myDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string result = ViecLamEntry.Text;
            if (result != null)
            {
                if (isAdd == true)
                {
                    ViecLam viecLam = new ViecLam
                    {
                        MaChuDe = _chuDe.MaChuDe,
                        NoiDungViecLam = result,
                        ThoiGian = timeEntry
                    };
                    AddEditViecLam(viecLam, isAdd);
                }
                else
                {
                    ViecLam viecLam = new ViecLam
                    {
                        MaViecLam = _viecLam.MaViecLam,
                        MaChuDe = _chuDe.MaChuDe,
                        NoiDungViecLam = result,
                        ThoiGian = timeEntry
                    };
                    AddEditViecLam(viecLam, isAdd);
                }
                
            }
        }

        private void CancelViecLamPopup_Clicked(object sender, EventArgs e)
        {
            popupAddView.IsVisible = false;
        }

        private async void ViecLamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViecLam selectedViecLam = (ViecLam)ViecLamList.SelectedItem;
            int result = selectedViecLam.IsDone;
            if (result == 1)
                result = 0;
            else
                result = 1;
            ViecLam viecLam = new ViecLam
            {
                MaViecLam = selectedViecLam.MaViecLam,
                IsDone = result
            };
            HttpClient http = new HttpClient();
            string jsonViecLam = JsonConvert.SerializeObject(viecLam);
            StringContent httcontent = new StringContent(jsonViecLam, Encoding.UTF8, "application/json");
            HttpResponseMessage kq = await http.PostAsync("http://" + App.ip + "/api/User/UpdateIsDone", httcontent);
            LayViecLam();
        }
    }
}