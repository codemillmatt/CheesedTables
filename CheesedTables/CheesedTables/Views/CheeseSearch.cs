using System;

using Xamarin.Forms;
using System.Threading.Tasks;

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

			searchBar.SearchButtonPressed += async (sender, e) => {
				var service = new CheeseTableService ();
				service.GetCheeseReview("A",Guid.Parse("e8a9b095-7cdc-48e0-87a6-ba4297b6f45b"));
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


