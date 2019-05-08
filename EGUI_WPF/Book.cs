using System;
using System.ComponentModel;

namespace EGUI_WPF
{
	public class Book : INotifyPropertyChanged
	{
		private string _author;
		private string _title;
		private DateTime _publicationDate;

		public string Author
		{
			get
			{
				return _author;
			}
			set
			{
				_author = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Author)));
			}
		}
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));

			}
		}
		public DateTime PublicationDate
		{
			get => _publicationDate; set
			{
				_publicationDate = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(PublicationDate)));

			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
