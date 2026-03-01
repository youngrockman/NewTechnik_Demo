using Avalonia.Controls;
using MsBox.Avalonia;
using System.Linq;
using test_demo.Models;

namespace test_demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Close();
        }

        private void Guest_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var catalog = new CatalogWindow();
            catalog.Show();
            this.Close();
        }

        private async void Auth_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            using var context = new DemoContext();

            var login = LoginBox.Text;
            var password = passwordBox.Text;

            var user = context.Users.FirstOrDefault(x=>x.Userlogin == login && x.Userpassword == password);

            if (user != null)
            {
                var catalog = new CatalogWindow(user.Userid);
                catalog.Show();
                this.Close();
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы ввели неверные данные", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                error.ShowAsync();
            }
        }
    }
}