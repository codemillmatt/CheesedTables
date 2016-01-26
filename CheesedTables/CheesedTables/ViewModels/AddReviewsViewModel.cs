using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;

namespace CheesedTables
{
	public class AddReviewsViewModel : INotifyPropertyChanged
	{
		INavigation Navigation;

		public AddReviewsViewModel (INavigation nav)
		{
			Navigation = nav;
		}

		string _email;

		public string Email {
			get { return _email; }
			set {
				_email = value;

				PropertyChanged (this, new PropertyChangedEventArgs (nameof (Email)));
				AddCommand.ChangeCanExecute ();
			}
		}

		string _cheeseType;

		public string CheeseType {
			get { return _cheeseType; }
			set {
				_cheeseType = value;
				PropertyChanged (this, new PropertyChangedEventArgs (nameof (CheeseType)));
				AddCommand.ChangeCanExecute ();
			}
		}

		string _dairyName;

		public string DairyName {
			get { return _dairyName; }
			set { 
				_dairyName = value;
				PropertyChanged (this, new PropertyChangedEventArgs (nameof (DairyName)));
				AddCommand.ChangeCanExecute ();
			}
		}

		string _comments;

		public string Comments {
			get { return _comments; }
			set {
				_comments = value;
				PropertyChanged (this, new PropertyChangedEventArgs (nameof (Comments)));
				AddCommand.ChangeCanExecute ();
			}
		}

		Command _addCommand;

		public Command AddCommand {
			get {
				if (_addCommand == null) {
					_addCommand = new Command (async () => await AddTableEntityAsync (), 
						() => CanExecuteAddCommand());
				}

				return _addCommand;
			}
		}

		async Task AddTableEntityAsync ()
		{
			var rowKey = Guid.NewGuid ();

			var entityToSave = new CheeseReviewEntity (this.Email, rowKey);
			entityToSave.CheeseType = CheeseType;
			entityToSave.Comments = Comments;
			entityToSave.DairyName = DairyName;
			entityToSave.ReviewDate = DateTime.Now;

			// First add to Akavache (ideally in Akavache we would also keep track of whether it uploaded successfully or not
			await BlobCache.LocalMachine.InsertObject<CheeseReviewEntity> (
				$"{entityToSave.PartitionKey}|{entityToSave.RowKey}",
				entityToSave
			);

			// Get the table service
			var tableService = new CheeseTableService ();
			bool success = await tableService.SaveReviewAsync (entityToSave);


			if (success) {
				// Could display a success or failure message

				// We could also keep track of whether the entity has been uploaded yet or not for offline access
			

				// Pop the modal off the stack
				await Navigation.PopModalAsync ();
			}
				
		}

		bool CanExecuteAddCommand() {
			return !(string.IsNullOrWhiteSpace (CheeseType) ||
			string.IsNullOrWhiteSpace (Email) ||
			string.IsNullOrWhiteSpace (DairyName) ||
			string.IsNullOrWhiteSpace (Comments)
			);
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}

