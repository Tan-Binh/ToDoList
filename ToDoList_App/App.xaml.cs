using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToDoList_App.Views;

[assembly: ExportFont("Montserrat-Bold.ttf",Alias="Montserrat-Bold")]
[assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
[assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
[assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
[assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]

namespace ToDoList_App
{
    public partial class App : Application
    {
        public static string ip = "todolist19521269.somee.com";
        public static int UserId = -1;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
