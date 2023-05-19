using LibraryAppWpf.DbModel;
using LibraryAppWpf.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryAppWpf.Model
{
    public class MainWindowViewModel : BaseViewModel
    {
        string _searchQuery = "";
        ObservableCollection<Book> _booksList = new ObservableCollection<Book>();
        Book? _selectedBook = null;

        string _searchOrderQuery = "";
        ObservableCollection<Order> _ordersList = new ObservableCollection<Order>();
        Order? _selectedOrder = null;

        string _searchStudentQuery = "";
        ObservableCollection<Student> _studentsList = new ObservableCollection<Student>();
        Student? _selectedStudent = null;

        string _searchStaffQuery = "";
        ObservableCollection<User> _staffList = new ObservableCollection<User>();
        User? _selectedStaff = null;

        public string Username { get => $"{Static.CurrentUser.Lastname} {Static.CurrentUser.Firstname}"; }

        public ObservableCollection<Book> BooksList
        {
            get => _booksList;
            set
            {
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
        public Order? ChosenOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(ChosenOrder));
            }
        }

        public ObservableCollection<Order> OrdersList
        {
            get => _ordersList;
            set
            {
                _ordersList = value;
                OnPropertyChanged(nameof(OrdersList));
            }
        }
        public string SearchOrderQuery
        {
            get => _searchOrderQuery;
            set
            {
                _searchOrderQuery = value;
                UpdateOrdersList();
                OnPropertyChanged(nameof(SearchOrderQuery));
            }
        }
        public Book? ChosenBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                IsEditable = _selectedBook is not null;
                OnPropertyChanged(nameof(IsEditable));
                OnPropertyChanged(nameof(ChosenBook));
            }
        }

        public ObservableCollection<Student> StudentsList
        {
            get => _studentsList;
            set
            {
                _studentsList = value;
                OnPropertyChanged(nameof(StudentsList));
            }
        }
        public string SearchStudentQuery
        {
            get => _searchStudentQuery;
            set
            {
                _searchStudentQuery = value;
                UpdateStudentsList();
                OnPropertyChanged(nameof(SearchStudentQuery));
            }
        }
        public Student? ChosenStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(ChosenStudent));
            }
        }

        public ObservableCollection<User> StaffList
        {
            get => _staffList;
            set
            {
                _staffList = value;
                OnPropertyChanged(nameof(StaffList));
            }
        }
        public string SearchStaffQuery
        {
            get => _searchStaffQuery;
            set
            {
                _searchStudentQuery = value;
                UpdateStaffList();
                OnPropertyChanged(nameof(SearchStaffQuery));
            }
        }
        public User? ChosenStaff
        {
            get => _selectedStaff;
            set
            {
                _selectedStaff = value;
                OnPropertyChanged(nameof(ChosenStaff));
            }
        }

        public Visibility IsUserAdmin { get => Static.CurrentUser.Role.Value == "ADMIN" ? Visibility.Visible : Visibility.Collapsed; }
        bool _isEditable = false;
        public bool IsEditable {
            get => _isEditable; 
            set
            {
                _isEditable = value;
                OnPropertyChanged(nameof(IsEditable));
            }
        }

        public MainWindowViewModel()
        {
            UpdateBooksList();
            UpdateOrdersList();
            UpdateStudentsList();
            UpdateStaffList();
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

        public void UpdateOrdersList()
        {
            using (var db = new libraryDatabaseContext())
            {
                OrdersList.Clear();
                foreach (var item in db.Order
                    .Include(r => r.Status)
                    .Include(r => r.Staff)
                    .ThenInclude(r => r.Role)
                    .Include(r => r.Student)
                    .Include(r => r.Book)
                    .ThenInclude(r => r.Author)
                    .ThenInclude(r => r.Country)
                    .Where(
                        b =>
                        b.Student.Firstname.Contains(SearchOrderQuery) ||
                        b.Student.Lastname.Contains(SearchOrderQuery) ||
                        b.Staff.Firstname.Contains(SearchOrderQuery) ||
                        b.Staff.Lastname.Contains(SearchOrderQuery) ||
                        b.TakingDate.ToString().Contains(SearchOrderQuery) ||
                        b.Book.Any(r => r.Keywords.Contains(SearchOrderQuery))))
                {
                    OrdersList.Add(item);
                }
            }
        }

        public void UpdateStudentsList()
        {
            using (var db = new libraryDatabaseContext())
            {
                StudentsList.Clear();
                foreach (var item in db.Student
                    .Where(
                        s =>
                        s.Firstname.Contains(SearchStudentQuery) ||
                        s.Lastname.Contains(SearchStudentQuery) ||
                        s.Middlename.Contains(SearchStudentQuery)))
                {
                    StudentsList.Add(item);
                }
            }
        }

        public void UpdateStaffList()
        {
            using (var db = new libraryDatabaseContext())
            {
                StaffList.Clear();
                foreach (var item in db.User
                    .Include(r => r.Role)
                    .Where(
                        s =>
                        s.Firstname.Contains(SearchStaffQuery) ||
                        s.Lastname.Contains(SearchStaffQuery) ||
                        s.Login.Contains(SearchStaffQuery)))
                {
                    StaffList.Add(item);
                }
            }
        }


        RelayCommand _removeBookCommand;
        public RelayCommand RemoveBookCommand
        {
            get
            {
                return _removeBookCommand ??
                    (_removeBookCommand = new RelayCommand(obj =>
                    {
                        var answer = MessageBox.Show(
                            $"Вы действительно хотите удалить книгу {ChosenBook.Name}?",
                            "Подтверждение",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);
                        if (answer == MessageBoxResult.Yes)
                        {
                            using (var db = new libraryDatabaseContext())
                            {
                                db.Book.Remove(ChosenBook);
                                db.SaveChanges();
                            }
                            UpdateBooksList();
                        }
                    }));
            }
        }

        RelayCommand _editBookCommand;
        public RelayCommand EditBookCommand
        {
            get
            {
                return _editBookCommand ??
                    (_editBookCommand = new RelayCommand(obj =>
                    {
                        if (ChosenBook is null) {
                            MessageBox.Show(
                                "Сначала выберите книгу",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var edit = new BookEditWindow(ChosenBook);
                            edit.ShowDialog();
                        }

                        UpdateBooksList();
                    }));
            }
        }
        RelayCommand _addBookCommand;
        public RelayCommand AddBookCommand
        {
            get
            {
                return _addBookCommand ??
                    (_addBookCommand = new RelayCommand(obj =>
                    {
                        var edit = new BookEditWindow();
                        edit.ShowDialog();

                        UpdateBooksList();
                    }));
            }
        }


        RelayCommand _removeStaffCommand;
        public RelayCommand RemoveStaffCommand
        {
            get
            {
                return _removeStaffCommand ??
                    (_removeStaffCommand = new RelayCommand(obj =>
                    {
                        if (ChosenStaff is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите пользователя",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var answer = MessageBox.Show(
                                $"Вы действительно хотите удалить пользователя {ChosenStaff.Login}?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                            if (answer == MessageBoxResult.Yes)
                            {
                                using (var db = new libraryDatabaseContext())
                                {
                                    if (ChosenStaff.Role.Value == "ADMIN")
                                    {
                                        MessageBox.Show(
                                            $"Нельзя удалить администратора {ChosenStaff.Login}",
                                            "Ошибка",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        db.User.Remove(ChosenStaff);
                                        db.SaveChanges();
                                    }

                                }
                                UpdateStaffList();
                            }
                        }
                    }));
            }
        }

        RelayCommand _editStaffCommand;
        public RelayCommand EditStaffCommand
        {
            get
            {
                return _editStaffCommand ??
                    (_editStaffCommand = new RelayCommand(obj =>
                    {
                        if (ChosenStaff is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите пользователя",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var edit = new StaffEditWindow(ChosenStaff);
                            edit.ShowDialog();
                        }

                        UpdateStaffList();
                    }));
            }
        }
        RelayCommand _addStaffCommand;
        public RelayCommand AddStaffCommand
        {
            get
            {
                return _addStaffCommand ??
                    (_addStaffCommand = new RelayCommand(obj =>
                    {
                        var edit = new StaffEditWindow();
                        edit.ShowDialog();

                        UpdateStaffList();
                    }));
            }
        }


        RelayCommand _removeStudentCommand;
        public RelayCommand RemoveStudentCommand
        {
            get
            {
                return _removeStudentCommand ??
                    (_removeStudentCommand = new RelayCommand(obj =>
                    {
                        if (ChosenStudent is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите ученика",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var answer = MessageBox.Show(
                                $"Вы действительно хотите удалить пользователя {ChosenStudent.Firstname} {ChosenStudent.Lastname}?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                            if (answer == MessageBoxResult.Yes)
                            {
                                using (var db = new libraryDatabaseContext())
                                {
                                    db.Student.Remove(ChosenStudent);
                                    db.SaveChanges();

                                }
                                UpdateStudentsList();
                            }
                        }
                    }));
            }
        }

        RelayCommand _editStudentCommand;
        public RelayCommand EditStudentCommand
        {
            get
            {
                return _editStudentCommand ??
                    (_editStudentCommand = new RelayCommand(obj =>
                    {
                        if (ChosenStudent is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите ученика",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var edit = new StudentEditWindow(ChosenStudent);
                            edit.ShowDialog();
                        }

                        UpdateStudentsList();
                    }));
            }
        }
        RelayCommand _addStudentCommand;
        public RelayCommand AddStudentCommand
        {
            get
            {
                return _addStudentCommand ??
                    (_addStudentCommand = new RelayCommand(obj =>
                    {
                        var edit = new StudentEditWindow();
                        edit.ShowDialog();

                        UpdateStudentsList();
                    }));
            }
        }


        RelayCommand _removeOrderCommand;
        public RelayCommand RemoveOrderCommand
        {
            get
            {
                return _removeOrderCommand ??
                    (_removeOrderCommand = new RelayCommand(obj =>
                    {
                        if (ChosenOrder is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите запись",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var answer = MessageBox.Show(
                                $"Вы действительно хотите удалить запись {ChosenOrder.Id}?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                            if (answer == MessageBoxResult.Yes)
                            {
                                using (var db = new libraryDatabaseContext())
                                {
                                    db.Order.Remove(ChosenOrder);
                                    db.SaveChanges();

                                }
                                UpdateStudentsList();
                            }
                        }
                    }));
            }
        }

        RelayCommand _editOrderCommand;
        public RelayCommand EditOrderCommand
        {
            get
            {
                return _editOrderCommand ??
                    (_editOrderCommand = new RelayCommand(obj =>
                    {
                        if (ChosenOrder is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите запись",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var edit = new RecordEditWindow(ChosenOrder);
                            edit.ShowDialog();
                        }

                        UpdateOrdersList();
                    }));
            }
        }
        RelayCommand _addOrderCommand;
        public RelayCommand AddOrderCommand
        {
            get
            {
                return _addOrderCommand ??
                    (_addOrderCommand = new RelayCommand(obj =>
                    {
                        if (ChosenBook is null)
                        {
                            MessageBox.Show(
                                "Сначала выберите книгу",
                                "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                        else
                        {
                            var edit = new RecordEditWindow(ChosenBook);
                            edit.ShowDialog();

                            UpdateOrdersList();
                        }

                    }));
            }
        }
    }
}
