using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EGUI_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private string _titleFilter;
		private string _authorFilter;
		private string _dateFilter;

		public string TitleFilter
		{
			get => _titleFilter;
			set
			{
				_titleFilter = value;
				Books.View.Refresh();
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TitleFilter)));
			}
		}
		public string AuthorFilter
		{
			get => _authorFilter;
			set
			{
				_authorFilter = value;
				Books.View.Refresh();
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AuthorFilter)));
			}
		}
		public string DateFilter
		{
			get => _dateFilter;
			set
			{
				_dateFilter = value;
				Books.View.Refresh();
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DateFilter)));
			}
		}

		private readonly ObservableCollection<Book> _books = new ObservableCollection<Book>();

		public event PropertyChangedEventHandler PropertyChanged;

		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			Books.Source = _books;
			BookListView.ItemsSource = Books.View;
			_books.Add(new Book());
			Books.View.Filter = x =>
			{
				var book = x as Book;
				return
				(string.IsNullOrEmpty(TitleFilter) || TitleFilter == book.Title) &&
				(string.IsNullOrEmpty(AuthorFilter) || AuthorFilter == book.Author) &&
				(DateFilter == null || book.PublicationDate.Year.ToString().StartsWith(DateFilter));
			};
		}
		public CollectionViewSource Books { get; set; } = new CollectionViewSource();

		private void Add_new_click(object sender, RoutedEventArgs e)
		{
			var window = new EditBookDialogVM(null);
			var dialog = new EditBookDialog()
			{
				DataContext = window
			};
			if (dialog.ShowDialog() ?? false)
			{
				_books.Add(window.Result);
			}
		}

		private void Delete_Selected_Click(object sender, RoutedEventArgs e)
		{
			while(BookListView.SelectedItems.Count > 0)
				_books.Remove(BookListView.SelectedItems[0] as Book);
		}

		private void Clear_Filter_Click(object sender, RoutedEventArgs e)
		{
			TitleFilter = null;
			AuthorFilter = null;
			DateFilter = null;
		}

		private void Edit_Book_Click(object sender, RoutedEventArgs e)
		{
			if (BookListView.SelectedItem is Book book)
			{
				var model = new EditBookDialogVM(book);
				var dialog = new EditBookDialog()
				{
					DataContext = model
				};
				dialog.ShowDialog();
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Books)));
			}

		}

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			using (var openFileDialog = new OpenFileDialog() { Filter = "book files|*.extension_i_came_up_with", AddExtension = true })
				if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					using (var stream = openFileDialog.OpenFile())
					{

						var deserializer = new XmlSerializer(_books.GetType());
						foreach (var book in deserializer.Deserialize(stream) as IEnumerable<Book>)
							_books.Add(book);
					}

				}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			using (var openFileDialog = new SaveFileDialog() { Filter = "book files|*.extension_i_came_up_with", AddExtension = true })
				if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					using (var stream = openFileDialog.OpenFile())
					{
						var deserializer = new XmlSerializer(_books.GetType());
						deserializer.Serialize(stream, _books);
					}
				}
		}
	}
}

