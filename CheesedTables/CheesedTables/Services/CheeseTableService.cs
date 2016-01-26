using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace CheesedTables
{
	public class CheeseTableService
	{
		readonly string reviewTable = "reviews";
		CloudTableClient client = null;

		// CheeseReviewEntity property names - kinda hacky, but works for now
		readonly string EmailAddressPropName = "EmailAddress";
		readonly string CheeseTypePropName = "CheeseType";
		readonly string DairyNamePropName = "DairyName";
		readonly string ReviewDatePropName = "ReviewDate";
		readonly string CommentsPropName = "Comments";

		public CheeseTableService ()
		{
			
		}

		public async Task<CheeseReviewEntity> GetCheeseReview (string emailAddress, Guid rowKey)
		{
			CheeseReviewEntity returnCheese = null;

			try {
				if (client == null)
					await InitializeCloudClientAsync ();

				// Define a table operation that grabs the exact partition & row key
				var op = TableOperation.Retrieve<CheeseReviewEntity> (emailAddress, rowKey.ToString ());

				// Get a table reference
				var table = client.GetTableReference (reviewTable);

				// Execute the table operation
				var result = await table.ExecuteAsync (op);

				// Parse the return - will need to cast it
				if (result.Result != null)
					returnCheese = (CheeseReviewEntity)result.Result;

			} catch (Exception ex) {
				returnCheese = null;
			}

			return returnCheese;
		}

		public async Task<List<CheeseReviewEntity>> SearchCheeseReviewsAsync (string emailAddress)
		{
			List<CheeseReviewEntity> results = new List<CheeseReviewEntity> ();

			try {
				if (client == null)
					await InitializeCloudClientAsync ();
						
				var table = client.GetTableReference (reviewTable);

				// Generate a query
				var query = new TableQuery ();
				query.Select (new List<string> () { 
					EmailAddressPropName,
					CheeseTypePropName,
					DairyNamePropName,
					ReviewDatePropName,
					CommentsPropName
				});
					
				// Looking for an exact match
				query.Where (TableQuery.GenerateFilterCondition ("PartitionKey", QueryComparisons.Equal, emailAddress));

				// This version of the Table Storage NuGet only has the option
				// of returning a subset of the entire entity set at a time. 
				// We could use the TableContinuation token along with a loop to get
				// everything out - but we'll just go with the first return
				TableContinuationToken continueToken = null;

				// Execute the query
				var s = await table.ExecuteQuerySegmentedAsync (query, continueToken);

				continueToken = s.ContinuationToken;

				var searchResults = s.Results;
		
				foreach (var item in searchResults) {
					var newCheeseResult = new CheeseReviewEntity ();

					// all the columns will be stored within the properties dictionary
					EntityProperty ep;

					ep = item.Properties [CheeseTypePropName];
					newCheeseResult.CheeseType = ep.StringValue;

					ep = item.Properties [CommentsPropName];
					newCheeseResult.Comments = ep.StringValue;

					ep = item.Properties [DairyNamePropName];
					newCheeseResult.DairyName = ep.StringValue;

					ep = item.Properties [EmailAddressPropName];
					newCheeseResult.EmailAddress = ep.StringValue;

					ep = item.Properties [ReviewDatePropName];
					if (ep.DateTime.HasValue)
						newCheeseResult.ReviewDate = ep.DateTime.Value;

					results.Add (newCheeseResult);
				}
			} catch (Exception ex) {
				var exMsg = ex.ToString ();
			}

			return results;
		}

		public async Task<bool> SaveReviewAsync (CheeseReviewEntity entity)
		{
			try {							
				if (client == null)
					await InitializeCloudClientAsync ();
					
				// Define the insert operation
				var operation = TableOperation.InsertOrReplace (entity);

				// Get a reference to the review table
				var table = client.GetTableReference (reviewTable);

				// Execute the insert against the table
				var result = await table.ExecuteAsync (operation);

				return true;

			} catch (Exception ex) {
				return false;
			}
		}

		private async Task InitializeCloudClientAsync ()
		{
			// First get the credentials used to access Azure Table Storage
			var credentials = await GetStorageCredentials ();

			// Create the CloudTableClient
			client = new CloudTableClient (new Uri (AzureConstants.TableServiceUrl), credentials);
		}

		private async Task<StorageCredentials> GetStorageCredentials ()
		{						
			// Just hitting a web service that returns a token in string form
			var wc = new HttpClient ();
			var token = await wc.GetStringAsync (AzureConstants.SasUrl);

			// Create the storage credentials
			return new StorageCredentials (token);			
		}
	}
}

