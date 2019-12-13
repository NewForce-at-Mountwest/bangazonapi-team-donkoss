using Newtonsoft.Json;
using BangazonAPI.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System;
namespace BangazonAPITest.TestControllers
{
    public class ProductTypeControllerTest
    {
        // This is going to be our test ProductType instance that we create and delete to make sure everything works
        private ProductType dummyProductType { get; } = new ProductType
        {
            Name = "TestType",
            Archived = false
        };
        // We'll store our base url for this route as a private field to avoid typos
        private string url { get; } = "/api/producttype";
        // Reusable method to create a new coffee in the database and return it
        public async Task<ProductType> CreateDummyProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Serialize the C# object into a JSON string
                string ProductTypeAsJSON = JsonConvert.SerializeObject(dummyProductType);
                // Use the client to send the request and store the response
                HttpResponseMessage response = await client.PostAsync(
                    url,
                    new StringContent(ProductTypeAsJSON, Encoding.UTF8, "application/json")
                );
                // Store the JSON body of the response
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON into an instance of a ProductType
                ProductType newlyCreatedProductType = JsonConvert.DeserializeObject<ProductType>(responseBody);
                return newlyCreatedProductType;
            }
        }
        // Reusable method to delete a ProductType from the database
        public async Task deleteDummyProductType(ProductType producttypeToDelete)
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                HttpResponseMessage deleteResponse = await client.DeleteAsync($"{url}/{producttypeToDelete.Id}");
            }
        }
        /* TESTS START HERE */
        [Fact]
        public async Task Create_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Create a new ProductType in the db
                ProductType newTestType = await CreateDummyProductType();
                // Try to get it again
                HttpResponseMessage response = await client.GetAsync($"{url}/{newTestType.Id}");
                response.EnsureSuccessStatusCode();
                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                // Turn the JSON into C#
                ProductType newProductType = JsonConvert.DeserializeObject<ProductType>(responseBody);
                // Make sure it's really there
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(dummyProductType.Name, newProductType.Name);
                Assert.Equal(dummyProductType.Archived, newProductType.Archived);
                // Clean up after ourselves
                await deleteDummyProductType(newProductType);
            }
        }
        [Fact]
        public async Task Delete_ProductType()
        {
            // Note: with many of these methods, I'm creating dummy data and then testing to see if I can delete it. I'd rather do that for now than delete something else I (or a user) created in the database, but it's not essential-- we could test deleting anything 
            // Create a new coffee in the db
            ProductType newTestType = await CreateDummyProductType();
            // Delete it
            await deleteDummyProductType(newTestType);
            using (var client = new APIClientProvider().Client)
            {
                // Try to get it again
                HttpResponseMessage response = await client.GetAsync($"{url}{newTestType.Id}");
                // Make sure it's really gone
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
        [Fact]
        public async Task Get_All_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Try to get all of the ProductTypes from /api/ProductType
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // Convert to JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                // Convert from JSON to C#
                List<ProductType> producttypes = JsonConvert.DeserializeObject<List<ProductType>>(responseBody);
                // Make sure we got back a 200 OK Status and that there are more than 0 ProductTypes in our database
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(producttypes.Count > 0);
            }
        }
        [Fact]
        public async Task Get_Single_ProductType()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                // Create a dummy coffee
                ProductType newTestType = await CreateDummyProductType();
                // Try to get it
                HttpResponseMessage response = await client.GetAsync($"{url}/{newTestType.Id}");
                response.EnsureSuccessStatusCode();
                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                // Turn the JSON into C#
                ProductType TestTypeFromDB = JsonConvert.DeserializeObject<ProductType>(responseBody);
                // Did we get back what we expected to get back? 
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(dummyProductType.Name, newTestType.Name);
                Assert.Equal(dummyProductType.Archived, newTestType.Archived);
                // Clean up after ourselves-- delete the dummy coffee we just created
                await deleteDummyProductType(TestTypeFromDB);
            }
        }
        [Fact]
        public async Task Update_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Create a dummy ProductType
                ProductType newTestType = await CreateDummyProductType();
                // Make a new Manufacturer and assign it to our dummy ProductType
                string newName = "TestType";
                newTestType.Name = newName;
                // Convert it to JSON
                string modifiedTestTypeAsJSON = JsonConvert.SerializeObject(newTestType);
                // Try to PUT the newly edited ProductType
                var response = await client.PutAsync(
                        $"{url}/{newTestType.Id}",
                        new StringContent(modifiedTestTypeAsJSON, Encoding.UTF8, "application/json")
                    );
                // See what comes back from the PUT. Is it a 204? 
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                // Get the edited ProductType back from the database after the PUT
                var getModifiedTestType = await client.GetAsync($"{url}/{newTestType.Id}");
                getModifiedTestType.EnsureSuccessStatusCode();
                // Convert it to JSON
                string getProductTypeBody = await getModifiedTestType.Content.ReadAsStringAsync();
                // Convert it from JSON to C#
                ProductType newlyEditedProductType = JsonConvert.DeserializeObject<ProductType>(getProductTypeBody);
                // Make sure the acctNumber was modified correctly
                Assert.Equal(HttpStatusCode.OK, getModifiedTestType.StatusCode);
                Assert.Equal(newName, newlyEditedProductType.Name);
                // Clean up after yourself
                await deleteDummyProductType(newlyEditedProductType);
            }
        }
        [Fact]
        public async Task Test_Get_NonExitant_ProductType_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Try to get a ProductType with an Id that could never exist
                HttpResponseMessage response = await client.GetAsync($"{url}/00000000");
                // It should bring back a 204 no content error
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
        [Fact]
        public async Task Test_Delete_NonExistent_ProductType_Fails()
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