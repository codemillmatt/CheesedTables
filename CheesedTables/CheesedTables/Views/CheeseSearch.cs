using System;

using Xamarin.Forms;

namespace CheesedTables
{
	public class CheeseSearch : ContentPage
	{
		ListView searchResults;
		SearchBar searchBar;

		public CheeseSearch ()
		{
			Title = "Search Cheese Reviews!";
			searchResults = new ListView ();
			searchBar = new SearchBar () { Placeholder = "Reviewer Email" };

			var addButtonItem = new ToolbarItem ();
			addButtonItem.Text = "Add";
			addButtonItem.Clicked += async (sender, e) => {
				var addReview = new AddReview();

				var navPage = new NavigationPage(addReview);

				await Navigation.PushModalAsync(navPage);
			};

			this.ToolbarItems.Add (addButtonItem);

			searchBar.SearchButtonPressed += (sender, e) => {

			};

			Content = new StackLayout { 
				Children = {
					searchBar,
					searchResults
				}
			};
		}
	}
}


