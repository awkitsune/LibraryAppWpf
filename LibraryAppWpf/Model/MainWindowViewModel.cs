using LibraryAppWpf.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryAppWpf.Model
{
    public class MainWindowViewModel : BaseViewModel
    {
        string _searchQuery = "";
        ObservableCollection<Book> _booksList = new ObservableCollection<Book>();
        public string Username { get => $"{Static.CurrentUser.Lastname} {Static.CurrentUser.Firstname}"; }
        public ObservableCollection<Book> BooksList 
        { 
            get => _booksList; 
            set { 
                _booksList = value;
                OnPropertyChanged(nameof(BooksList));
            }
        }
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                UpdateBooksList();
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public MainWindowViewModel()
        {
            UpdateBooksList();
        }

        public void UpdateBooksList()
        {
            using (var db = new libraryDatabaseContext())
            {
                BooksList.Clear();
                foreach (var item in db.Book
                    .Include(r => r.Author)
                    .ThenInclude(r => r.Country)
                    .Where(
                        b =>
                        b.Name.Contains(SearchQuery) ||
                        b.Author.Fullname.Contains(SearchQuery) ||
                        b.Author.Country.Value.Contains(SearchQuery) ||
                        b.Keywords.Contains(SearchQuery)))
                {
                    BooksList.Add(item);
                }         
            }
        }
    }
}
