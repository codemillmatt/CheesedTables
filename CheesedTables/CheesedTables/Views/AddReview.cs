using System;

using Xamarin.Forms;

namespace CheesedTables
{
	public class AddReview : ContentPage
	{
		public AddReview ()
		{
			Title = "Add New Review";

			var email = new Entry () { Placeholder = "Email Address" };
			var cheeseType = new Entry () { Placeholder = "Cheese Type" };
			var dairy = new Entry () { Placeholder = "Dairy" };
			var comments = new Editor ();
			comments.BackgroundColor = Color.FromRgb (255, 255, 229);
			comments.VerticalOptions = LayoutOptions.FillAndExpand;

			var saveButton = new ToolbarItem () { Text = "Save" };
			saveButton.Clicked += async (object sender, EventArgs e) => {
				// Create a new review 
				var review = new Review();
				review.CheeseType = cheeseType.Text;
				review.Comments = comments.Text;
				review.DairyName = dairy.Text;
				review.EmailAddress = email.Text;
				review.ReviewDate = DateTime.Now;

				var entity = new CheeseReviewEntity(review);

				var service = new CheeseTableService();
				service.SaveReview(entity);

				await Navigation.PopModalAsync();
			};

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


