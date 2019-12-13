using Newtonsoft.Json;
using BangazonAPI.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BangazonAPITest.TestControllers
{
    public class PaymentTypeControllerTest
    {

            // This is going to be our test PaymentType instance that we create and delete to make sure everything works
            private PaymentType dummyPaymentType { get; } = new PaymentType
            {
                AcctNumber = 324,
                Name = "Bitcash",
                CustomerId = 1,
                Archived = false
            };

            // We'll store our base url for this route as a private field to avoid typos
            private string url { get; } = "/api/paymentType";


            // Reusable method to create a new coffee in the database and return it
            public async Task<PaymentType> CreateDummyPaymentType()
            {

                using (var client = new APIClientProvider().Client)
                {

                    // Serialize the C# object into a JSON string
                    string bitCashAsJSON = JsonConvert.SerializeObject(dummyPaymentType);


                    // Use the client to send the request and store the response
                    HttpResponseMessage response = await client.PostAsync(
                        url,
                        new StringContent(bitCashAsJSON, Encoding.UTF8, "application/json")
                    );

                    // Store the JSON body of the response
                    string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON into an instance of PaymentType
                PaymentType newlyCreatedPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                    return newlyCreatedPaymentType;
                }
            }

        // Reusable method to deelte a PaymentType from the database
        public async Task deleteDummyPaymentType(PaymentType paymentTypeToDelete)
            {
                using (HttpClient client = new APIClientProvider().Client)
                {
                    HttpResponseMessage deleteResponse = await client.DeleteAsync($"{url}/{paymentTypeToDelete.Id}");

                }

            }


            /* TESTS START HERE */


            [Fact]
            public async Task Create_PaymentType()
            {
                using (var client = new APIClientProvider().Client)
                {
                // Create a new PaymentType in the db
                PaymentType newBitCash = await CreateDummyPaymentType();

                    // Try to get it again
                    HttpResponseMessage response = await client.GetAsync($"{url}/{newBitCash.Id}");
                    response.EnsureSuccessStatusCode();

                    // Turn the response into JSON
                    string responseBody = await response.Content.ReadAsStringAsync();

                // Turn the JSON into C#
                PaymentType newPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                    // Make sure it's really there
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(dummyPaymentType.AcctNumber, newPaymentType.AcctNumber);
                    Assert.Equal(dummyPaymentType.Name, newPaymentType.Name);
                    Assert.Equal(dummyPaymentType.CustomerId, newPaymentType.CustomerId);
                    Assert.Equal(dummyPaymentType.Archived, newPaymentType.Archived);

                // Clean up after ourselves
                await deleteDummyPaymentType(newPaymentType);

                }

            }


            [Fact]

            public async Task Delete_PaymentType()
            {
            // Note: with many of these methods, I'm creating dummy data and then testing to see if I can delete it. I'd rather do that for now than delete something else I (or a user) created in the database, but it's not essential-- we could test deleting anything 

            // Create a new coffee in the db
            PaymentType newBitCash = await CreateDummyPaymentType();

                // Delete it
                await deleteDummyPaymentType(newBitCash);

                using (var client = new APIClientProvider().Client)
                {
                    // Try to get it again
                    HttpResponseMessage response = await client.GetAsync($"{url}{newBitCash.Id}");

                    // Make sure it's really gone
                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                }
            }

            [Fact]
            public async Task Get_All_PaymentType()
            {

                using (var client = new APIClientProvider().Client)
                {

                // Try to get all of the PaymentTypes from /api/PaymentType
                HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Convert to JSON
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Convert from JSON to C#
                    List<PaymentType> paymentTypes = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);

                // Make sure we got back a 200 OK Status and that there are more than 0 PaymentTypes in our database
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.True(paymentTypes.Count > 0);

                }
            }

            [Fact]
            public async Task Get_Single_PaymentType()
            {
                using (HttpClient client = new APIClientProvider().Client)
                {
                // Create a dummy coffee
                PaymentType newBitCash = await CreateDummyPaymentType();

                    // Try to get it
                    HttpResponseMessage response = await client.GetAsync($"{url}/{newBitCash.Id}");
                    response.EnsureSuccessStatusCode();

                    // Turn the response into JSON
                    string responseBody = await response.Content.ReadAsStringAsync();

                // Turn the JSON into C#
                PaymentType bitCashFromDB = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                    // Did we get back what we expected to get back? 
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(dummyPaymentType.AcctNumber, newBitCash.AcctNumber);
                    Assert.Equal(dummyPaymentType.Name, newBitCash.Name);
                    Assert.Equal(dummyPaymentType.CustomerId, newBitCash.CustomerId);
                    Assert.Equal(dummyPaymentType.Archived, newBitCash.Archived);

                // Clean up after ourselves-- delete the dummy coffee we just created
                await deleteDummyPaymentType(bitCashFromDB);

                }
            }




            [Fact]
            public async Task Update_PaymentType()
            {

                using (var client = new APIClientProvider().Client)
                {
                // Create a dummy PaymentType
                PaymentType newBitCash = await CreateDummyPaymentType();

                // Make a new accctNumber and assign it to our dummy PaymentType
                int newAcctNumber = 822;
                    newBitCash.AcctNumber = newAcctNumber;

                    // Convert it to JSON
                    string modifiedBitCashAsJSON = JsonConvert.SerializeObject(newBitCash);

                // Try to PUT the newly edited PaymentType
                var response = await client.PutAsync(
                        $"{url}/{newBitCash.Id}",
                        new StringContent(modifiedBitCashAsJSON, Encoding.UTF8, "application/json")
                    );

                    // See what comes back from the PUT. Is it a 204? 
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                // Get the edited PaymentType back from the database after the PUT
                var getModifiedBitCash = await client.GetAsync($"{url}/{newBitCash.Id}");
                    getModifiedBitCash.EnsureSuccessStatusCode();

                    // Convert it to JSON
                    string getPaymentTypeBody = await getModifiedBitCash.Content.ReadAsStringAsync();

                // Convert it from JSON to C#
                PaymentType newlyEditedPaymentType = JsonConvert.DeserializeObject<PaymentType>(getPaymentTypeBody);

                    // Make sure the acctNumber was modified correctly
                    Assert.Equal(HttpStatusCode.OK, getModifiedBitCash.StatusCode);
                    Assert.Equal(newAcctNumber, newlyEditedPaymentType.AcctNumber);

                    // Clean up after yourself
                    await deleteDummyPaymentType(newlyEditedPaymentType);
                }
            }

            [Fact]
            public async Task Test_Get_NonExitant_PaymentType_Fails()
            {

                using (var client = new APIClientProvider().Client)
                {
                // Try to get a PaymentType with an Id that could never exist
                HttpResponseMessage response = await client.GetAsync($"{url}/00000000");

                    // It should bring back a 204 no content error
                    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                }
            }

            [Fact]
            public async Task Test_Delete_NonExistent_PaymentType_Fails()
            {
                using (var client = new APIClientProvider().Client)
                {
                    // Try to delete an Id that shouldn't exist 
                    HttpResponseMessage deleteResponse = await client.DeleteAsync($"{url}0000000000");

                    Assert.False(deleteResponse.IsSuccessStatusCode);
                    Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
                }
            }
        }
    }

