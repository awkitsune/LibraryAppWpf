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
    public partial class StudentEditWindow : Window
    {
        Student _editableStudent;
        public StudentEditWindow(Student? _student = null)
        {
            InitializeComponent();

            if (_student != null)
            {
                _editableStudent = _student;
            }
            else
            {
                _editableStudent = new Student();
            }

            this.DataContext = _editableStudent;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new libraryDatabaseContext())
            {
                try
                {
                    db.Student.Update(_editableStudent);
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
