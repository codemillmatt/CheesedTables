using System;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace CheesedTables
{
	public class CheeseReviewEntity : TableEntity
	{
		public CheeseReviewEntity (string emailAddress, Guid rowKey)
		{
			this.PartitionKey = emailAddress;
			this.RowKey = rowKey.ToString ();
		}

		public CheeseReviewEntity ()
		{
			
		}
			
		public string EmailAddress {
			get;
			set;
		}

		public string CheeseType {
			get;
			set;
		}

		public string DairyName {
			get;
			set;
		}

		public DateTime ReviewDate {
			get;
			set;
		}

		public string Comments {
			get;
			set;
		}
	}
}

