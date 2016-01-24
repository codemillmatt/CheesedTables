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


