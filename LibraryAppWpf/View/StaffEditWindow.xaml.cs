using LibraryAppWpf.DbModel;
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
    /// Interaction logic for BookEditWindow.xaml
    /// </summary>
    public partial class StaffEditWindow : Window
    {
        User _editableUser;
        public StaffEditWindow(User? _user = null)
        {
            InitializeComponent();

            if (_user != null)
            {
                _editableUser = _user;
            }
            else
            {
                _editableUser = new User();

                _editableUser.Role = new Role();
            }

            this.DataContext = _editableUser;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new libraryDatabaseContext())
            {
                try
                {
                    if (_editableUser.Role.Value == null)
                    {
                        _editableUser.Role = db.Role.AsNoTracking().FirstOrDefault(a => a.Value.Equals("STAFF"));
                    }

                    db.User.Update(_editableUser);
                    db.SaveChanges();

                    this.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(
                        $"Произошла ошибка. \nПроверьте правильность заполнения данных!\n\n" +
                        $"{err.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

            }
            
        }
    }
}
