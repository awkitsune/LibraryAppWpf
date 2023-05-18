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
        Book _editableBook;
        public StudentEditWindow(Book? _book = null)
        {
            InitializeComponent();

            if (_book != null)
            {
                _editableBook = _book;
            }
            else
            {
                _editableBook = new Book();

                _editableBook.Author = new BookAuthor();
                _editableBook.Author.Country = new Country();
            }

            this.DataContext = _editableBook;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new libraryDatabaseContext())
            {
                if (db.Country.AsNoTracking().FirstOrDefault(a => a.Value.Equals(_editableBook.Author.Country.Value)) is null)
                {
                    db.Country.Add(_editableBook.Author.Country);
                    db.SaveChanges();
                }


                if (db.BookAuthor.AsNoTracking().FirstOrDefault(a => a.Fullname.Equals(_editableBook.Author.Fullname)) is null)
                {
                    db.BookAuthor.Add(_editableBook.Author);
                    db.SaveChanges();
                }

                db.Book.Update(_editableBook);
                db.SaveChanges();

                this.Close();
            }
            
        }
    }
}
