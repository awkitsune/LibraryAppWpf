using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryAppWpf.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void authBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text.Length < 1 || passBox.Password.Length < 1)
            {
                MessageBox.Show(
                    "Логин или пароль не должны быть пустыми",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else using (var db = new DbModel.libraryDatabaseContext())
                {
                    var user = await db.User.FirstOrDefaultAsync(u => u.Login.Equals(loginBox.Text) && u.Password.Equals(passBox.Password));
                    if (user == null)
                    {
                        MessageBox.Show(
                            "Проверьте правильность заполнения данных",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        passBox.Password = "";
                    }
                    else
                    {
                        Static.CurrentUser = user;

                        var mainWindow = new MainWindow();
                        this.Hide();
                        mainWindow.ShowDialog();
                        this.Show();
                    }
                }
        }
    }
}
