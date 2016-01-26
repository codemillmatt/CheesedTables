using System;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace CheesedTables
{
	public class CheeseSearchView : ContentPage
	{
		ListView searchResults;
		SearchBar searchBar;

		SearchReviewsViewModel viewModel;

		public CheeseSearchView ()
		{
			viewModel = new SearchReviewsViewModel ();

			this.BindingContext = viewModel;

			Title = "Search Cheese Reviews!";
			searchResults = new ListView ();
			searchBar = new SearchBar () { Placeholder = "Reviewer Email" };

			// Search bar doesn't seem to want to bind 
			// to the command parameter for some reason
			// hack our way to make it work
			searchBar.TextChanged += (sender, e) => {
				viewModel.EmailAddress = ((SearchBar)sender).Text;
			};

			searchResults.SetBinding (ListView.ItemsSourceProperty, "Reviews");

			var cell = new DataTemplate (typeof(TextCell));
			cell.SetBinding (TextCell.TextProperty, "Display");

			searchResults.ItemTemplate = cell;


			var addButtonItem = new ToolbarItem ();
			addButtonItem.Text = "Add";
			addButtonItem.Clicked += async (sender, e) => {
				var addReview = new AddReviewView();

				var navPage = new NavigationPage(addReview);

				await Navigation.PushModalAsync(navPage);
			};

			this.ToolbarItems.Add (addButtonItem);

			searchBar.SearchCommandParameter = searchBar.Text;
			searchBar.SearchCommand = viewModel.SearchReviewsCommand;

			Content = new StackLayout { 
				Children = {
					searchBar,
					searchResults
				}
			};
		}
	}
}


