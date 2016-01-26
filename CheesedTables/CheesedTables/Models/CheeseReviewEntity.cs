using System;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Collections;

namespace CheesedTables
{
	class CheeseReviewComparer : IComparer<CheeseReviewEntity> {

		#region IComparer implementation

		public int Compare (CheeseReviewEntity x, CheeseReviewEntity y)
		{
			return x.RowKey.CompareTo (y.RowKey);
		}

		#endregion

	}

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

		public string Display {
			get {
				return $"{EmailAddress} - {CheeseType}";
			}
		}

		public override string ToString ()
		{
			return $"{EmailAddress} - {CheeseType}";
		}
	}
}

