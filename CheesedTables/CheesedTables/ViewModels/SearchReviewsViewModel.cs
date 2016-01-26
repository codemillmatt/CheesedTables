using System;
using Microsoft.WindowsAzure.Storage;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akavache;
using System.Reactive.Linq;
using System.ComponentModel;

namespace CheesedTables
{
	public class SearchReviewsViewModel : INotifyPropertyChanged
	{		
		public SearchReviewsViewModel ()
		{
			Reviews = new ObservableCollection<CheeseReviewEntity> ();
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public ObservableCollection<CheeseReviewEntity> Reviews {
			get;
			set;
		}

		string _emailAddress;
		public string EmailAddress {
			get { 
				return _emailAddress;
			}
			set {
				_emailAddress = value;				
			}
		}
			
		Command _searchReviewsCommand;
		public Command SearchReviewsCommand {
			get {
				if (_searchReviewsCommand == null) {
					_searchReviewsCommand = new Command (
						async () => await SearchReviewsAsync (), 
						() => !string.IsNullOrWhiteSpace (EmailAddress));
				}

				return _searchReviewsCommand;
			}
		}

		private async Task SearchReviewsAsync()
		{
			// Search the reviews
			var tableService = new CheeseTableService();
			var searchResults = await tableService.SearchCheeseReviewsAsync(EmailAddress);

			// Loop through the results and place into Akavache
			foreach (var item in searchResults) {
				// Akavache will auto update when a key is matched
				await BlobCache.LocalMachine.InsertObject<CheeseReviewEntity> (
					$"{item.PartitionKey}|{item.RowKey}",
					item);

				// Check to see if this is already in the collection
				if (Reviews.Contains (item)) {
					Reviews.Remove (item);
				}
					
				Reviews.Add (item);
				
			}				
		}			
	}
}

