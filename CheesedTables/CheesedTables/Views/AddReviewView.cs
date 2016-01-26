using System;

using Xamarin.Forms;

namespace CheesedTables
{
	public class AddReviewView : ContentPage
	{
		AddReviewsViewModel viewModel;

		public AddReviewView ()
		{
			viewModel = new AddReviewsViewModel (Navigation);
			BindingContext = viewModel;

			Title = "Add New Review";

			var email = new Entry () { Placeholder = "Email Address" };
			email.SetBinding (Entry.TextProperty, "Email");

			var cheeseType = new Entry () { Placeholder = "Cheese Type" };
			cheeseType.SetBinding (Entry.TextProperty, "CheeseType");

			var dairy = new Entry () { Placeholder = "Dairy" };
			dairy.SetBinding (Entry.TextProperty, "DairyName");

			var comments = new Editor ();
			comments.BackgroundColor = Color.FromRgb (255, 255, 229);
			comments.VerticalOptions = LayoutOptions.FillAndExpand;
			comments.SetBinding (Editor.TextProperty, "Comments");

			var saveButton = new ToolbarItem () { Text = "Save" };

			saveButton.SetBinding (ToolbarItem.CommandProperty, "AddCommand");
					
			ToolbarItems.Add (saveButton);
					
			Content = new StackLayout { 
				Padding= new Thickness(10,10,10,10),
				Children = {					
					email,
					cheeseType,
					dairy,
					new Label{ Text = "Comments:" },
					comments
				}
			};
		}
	}
}


