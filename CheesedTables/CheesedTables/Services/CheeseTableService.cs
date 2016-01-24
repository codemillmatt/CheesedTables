using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;
using System.Collections.Generic;

namespace CheesedTables
{
	public class CheeseTableService
	{
		public CheeseTableService ()
		{
		}

		public async Task<List<CheeseReviewEntity>> SearchCheeseReviews(string emailAddress)
		{
			throw new NotImplementedException ();
		}

		public async Task SaveReview(CheeseReviewEntity entity)
		{
			throw new NotImplementedException ();
		}

		private async Task<StorageCredentials> GetStorageCredentials ()
		{
			try {
				// First get the SAS Token from Mobile Services API
				var mobileServiceClient = new MobileServiceClient (AzureConstants.MobileServiceUrl, 							
					AzureConstants.MobileServiceAppKey);

				var requestDict = new Dictionary<string,string> ();

				// Invoke the custom API in the mobile services to get the storage SAS token
				var token = await mobileServiceClient
					.InvokeApiAsync<string> (AzureConstants.MobileServiceAPISasName, 
						HttpMethod.Get, requestDict);

				// Create the storage credentials
				return new StorageCredentials (token);
			} catch (Exception ex) {				
				return null;
			}
		}
	}
}

