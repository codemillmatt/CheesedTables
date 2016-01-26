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

		public CheeseTableService ()
		{
			
		}

		public async Task<CheeseReviewEntity> GetCheeseReview(string emailAddress, Guid rowKey)
		{
			CheeseReviewEntity returnCheese = null;

			try {
				if (client == null)
					await InitializeCloudClientAsync();

				// Define a table operation that grabs the exact partition & row key
				var op = TableOperation.Retrieve<CheeseReviewEntity>(emailAddress, rowKey.ToString());

				// Get a table reference
				var table = client.GetTableReference(reviewTable);

				// Execute the table operation
				var result = await table.ExecuteAsync(op);

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
			try {
				if (client == null)
					await InitializeCloudClientAsync();
						

				var table = client.GetTableReference (reviewTable);

//				var op = TableOperation.Retrieve<CheeseReviewEntity> ("A", "A");
//
//				var result = await table.ExecuteAsync (op);
//
//				var s = result.Etag;

				var q = new TableQuery ();
				//q.Select(new List<string>() { 
				q.Where (TableQuery.GenerateFilterCondition ("PartitionKey", QueryComparisons.Equal, "A"));

				TableContinuationToken continueToken = null;

				var s = await table.ExecuteQuerySegmentedAsync (q, continueToken);

				continueToken = s.ContinuationToken;

				var results = s.Results;
		
				foreach (var item in results) {
					var asdf = item.ToString ();
				}
			} catch (Exception ex) {
				var s1 = ex.ToString ();

			}

			return new List<CheeseReviewEntity> ();
		}

		public async Task<bool> SaveReviewAsync (CheeseReviewEntity entity)
		{
			try {							
				if (client == null) 
					await InitializeCloudClientAsync();
					
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

		private async Task InitializeCloudClientAsync()
		{
			// First get the credentials used to access Azure Table Storage
			var credentials = await GetStorageCredentials ();

			// Create the CloudTableClient
			client = new CloudTableClient(new Uri(AzureConstants.TableServiceUrl), credentials);
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

