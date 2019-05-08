using System;
using System.Windows;
using System.Windows.Input;

namespace EGUI_WPF
{
	class EditBookDialogVM
	{
		private readonly Book _book;
		public EditBookDialogVM(Book book)
		{
			_book = book ?? new Book();
			_author = _book.Author;
			_title = _book.Title;
			PublicationDate = book.PublicationDate;
		}

		public Book Result { get => _book; }

		public ICommand OnOk
		{
			get
			{
				if (_onOk == null)
					_onOk = new RelayCommand(p => OnOkMethod(p), p=>IsValid(p));
				return _onOk;
			}
		}

		private bool IsValid(object _) => !string.IsNullOrWhiteSpace(_author) && !string.IsNullOrWhiteSpace(_title);

		private void OnOkMethod(object p)
		{
			_book.Author = Author;
			_book.Title = Title;
			_book.PublicationDate = PublicationDate;
			(p as Window).DialogResult = true;
			(p as Window).Close();
		}

		private void OnCancelMethod(object p)
		{
			(p as Window).DialogResult = false;
			(p as Window).Close();
		}

		RelayCommand _onOk;
		public ICommand OnCancel
		{
			get
			{
				if (_onCancel == null)
					_onCancel = new RelayCommand(p => OnCancelMethod(p));
				return _onCancel;
			}
		}

		private string _author;
		private string _title;
		private ICommand _onCancel;

		public string Author
		{
			get
			{
				return _author;
			}

			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new Exception();
				_author = value;
				_onOk.RecheckCanExecute();
			}
		}
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new Exception();
				_title = value;
				_onOk.RecheckCanExecute();
			}
		}
		public DateTime PublicationDate { get; set; }
	}
}
