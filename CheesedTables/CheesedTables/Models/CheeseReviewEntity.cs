using System;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace CheesedTables
{
	public class CheeseReviewEntity : TableEntity
	{
		public CheeseReviewEntity (Review cheeseReview)
		{
			this.PartitionKey = cheeseReview.CheeseType;
			this.RowKey = cheeseReview.EmailAddress;

			CheeseReviews = new List<Review> () { cheeseReview };
		}

		public List<Review> CheeseReviews {
			get;
			set;
		}
	}
}

