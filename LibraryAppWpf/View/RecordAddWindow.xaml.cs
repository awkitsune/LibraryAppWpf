using LibraryAppWpf.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class RecordAddWindow : Window
    {
        Order _editableOrder;
        Book? _addedBook = null;
        ObservableCollection<Student> studentsList = new ObservableCollection<Student>();
        ObservableCollection<OrderStatus> statusList = new ObservableCollection<OrderStatus>();

        public RecordAddWindow(Book _book)
        {
            InitializeComponent();

            using (var db = new libraryDatabaseContext())
            {
                foreach (var item in db.Student)
                {
                    studentsList.Add(item);
                }
                studentsCombo.ItemsSource = studentsList;

                foreach (var item in db.OrderStatus)
                {
                    statusList.Add(item);
                }
                statusCombo.ItemsSource = statusList;
            }

            _addedBook = _book;

            _editableOrder = new Order();

            _editableOrder.Student = new Student();
            _editableOrder.Status = new OrderStatus();
            _editableOrder.Book = new Collection<Book>() { _addedBook };
            _editableOrder.Staff = Static.CurrentUser;

            this.DataContext = _editableOrder;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new libraryDatabaseContext())
            {
                try
                {
                    db.Order.Add(_editableOrder);

                    db.SaveChanges();

                    this.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(
                        $"Произошла ошибка. \nПроверьте правильность заполнения данных!\n\n" +
                        $"{err}", 
                        "Ошибка", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
            }
            
        }
    }
}
