//**********************************************************************************************//
// Test for GET, GET ALL, POST, PUT, and DELETE
//**********************************************************************************************//

using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;


namespace BangazonAPITest
{
    public class ProductTest
    {

        // This is going to be our test coffee instance that we create and delete to make sure everything works
        private Product dummyProduct { get; } = new Product
        {
            ProductTypeId = 1,
            CustomerId = 2,
            Price = 30.00M,
            Title = "Abrams Tanks",
            Description = "Destructive machines of ultimate destruction.",
            Quantity = 500,
            Archived = false
        };

        // We'll store our base url for this route as a private field to avoid typos
        private string url { get; } = "/api/product";


        // Reusable method to create a new coffee in the database and return it
        public async Task<Product> CreateDummyProduct()
        {

            using (var client = new APIClientProvider().Client)
            {

                // Serialize the C# object into a JSON string
                string dummyProductAsJSON = JsonConvert.SerializeObject(dummyProduct);


                // Use the client to send the request and store the response
                HttpResponseMessage response = await client.PostAsync(
                    url,
                    new StringContent(dummyProductAsJSON, Encoding.UTF8, "application/json")
                );

                // Store the JSON body of the response
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON into an instance of Product
                Product newlyCreatedProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                return newlyCreatedProduct;
            }
        }

        // Reusable method to deelte a coffee from the database
        public async Task deleteDummyProduct(Product productToDelete)
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                HttpResponseMessage deleteResponse = await client.DeleteAsync($"{url}/{productToDelete.Id}");

            }

        }


        /* TESTS START HERE */


        [Fact]
        public async Task Create_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Create a new coffee in the db
                Product newAbramsTank = await CreateDummyProduct();

                // Try to get it again
                HttpResponseMessage response = await client.GetAsync($"{url}/{newAbramsTank.Id}");
                response.EnsureSuccessStatusCode();

                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Turn the JSON into C#
                Product newProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                // Make sure it's really there
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(dummyProduct.ProductTypeId, newProduct.ProductTypeId);
                Assert.Equal(dummyProduct.CustomerId, newProduct.CustomerId);
                Assert.Equal(dummyProduct.Price, newProduct.Price);
                Assert.Equal(dummyProduct.Title, newProduct.Title);
                Assert.Equal(dummyProduct.Description, newProduct.Description);
                Assert.Equal(dummyProduct.Quantity, newProduct.Quantity);
                Assert.Equal(dummyProduct.Archived, newProduct.Archived);


                //    ProductTypeId = 1,
                //CustomerId = 2,
                //Price = 30.00M,
                //Title = "Abrams Tanks",
                //Description = "Destructive machines of ultimate destruction.",
                //Quantity = 500,
                //Archived = false

                // Clean up after ourselves
                await deleteDummyProduct(newProduct);

            }

        }


        [Fact]

        public async Task Delete_Product()
        {
            // Note: with many of these methods, I'm creating dummy data and then testing to see if I can delete it. I'd rather do that for now than delete something else I (or a user) created in the database, but it's not essential-- we could test deleting anything 

            // Create a new coffee in the db
            Product newAbramsTank = await CreateDummyProduct();

            // Delete it
            await deleteDummyProduct(newAbramsTank);

            using (var client = new APIClientProvider().Client)
            {
                // Try to get it again
                HttpResponseMessage response = await client.GetAsync($"{url}{newAbramsTank.Id}?HardDelete=true");

                // Make sure it's really gone
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            }
        }

        [Fact]
        public async Task Get_All_Product()
        {

            using (var client = new APIClientProvider().Client)
            {

                // Try to get all of the products from /api/products
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Convert to JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Convert from JSON to C#
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

                // Make sure we got back a 200 OK Status and that there are more than 0 products in our database
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(products.Count > 0);

            }
        }

        [Fact]
        public async Task Get_Single_Product()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                // Create a dummy coffee
                Product newAbramsTank = await CreateDummyProduct();

                // Try to get it
                HttpResponseMessage response = await client.GetAsync($"{url}/{newAbramsTank.Id}");
                response.EnsureSuccessStatusCode();

                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Turn the JSON into C#
                Product abramsTankFromDB = JsonConvert.DeserializeObject<Product>(responseBody);

                // Did we get back what we expected to get back? 
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(dummyProduct.ProductTypeId, abramsTankFromDB.ProductTypeId);
                Assert.Equal(dummyProduct.CustomerId, abramsTankFromDB.CustomerId);
                Assert.Equal(dummyProduct.Price, abramsTankFromDB.Price);
                Assert.Equal(dummyProduct.Title, abramsTankFromDB.Title);
                Assert.Equal(dummyProduct.Description, abramsTankFromDB.Description);
                Assert.Equal(dummyProduct.Quantity, abramsTankFromDB.Quantity);
                Assert.Equal(dummyProduct.Archived, abramsTankFromDB.Archived);

                // Clean up after ourselves-- delete the dummy coffee we just created
                await deleteDummyProduct(abramsTankFromDB);

            }
        }




        [Fact]
        public async Task Update_Product()
        {

            using (var client = new APIClientProvider().Client)
            {
                // Create a dummy coffee
                Product newAbramsTank = await CreateDummyProduct();

                // Make a new title and assign it to our dummy coffee
                string newTitle = "HUGE FLIPPIN TANK OMG";
                newAbramsTank.Title = newTitle;

                // Convert it to JSON
                string modifiedAbramsTankAsJSON = JsonConvert.SerializeObject(newAbramsTank);

                // Try to PUT the newly edited coffee
                var response = await client.PutAsync(
                    $"{url}/{newAbramsTank.Id}",
                    new StringContent(modifiedAbramsTankAsJSON, Encoding.UTF8, "application/json")
                );

                // See what comes back from the PUT. Is it a 204? 
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                // Get the edited coffee back from the database after the PUT
                var getModifiedProduct = await client.GetAsync($"{url}/{newAbramsTank.Id}");
                getModifiedProduct.EnsureSuccessStatusCode();

                // Convert it to JSON
                string getProductBody = await getModifiedProduct.Content.ReadAsStringAsync();

                // Convert it from JSON to C#
                Product newlyEditedProduct = JsonConvert.DeserializeObject<Product>(getProductBody);

                // Make sure the title was modified correctly
                Assert.Equal(HttpStatusCode.OK, getModifiedProduct.StatusCode);
                Assert.Equal(newTitle, newlyEditedProduct.Title);

                // Clean up after yourself
                await deleteDummyProduct(newlyEditedProduct);
            }
        }

        [Fact]
        public async Task Test_Get_NonExitant_Product_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                // Try to get a coffee with an Id that could never exist
                HttpResponseMessage response = await client.GetAsync($"{url}/00000000");

                // It should bring back a 204 no content error
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistent_Product_Fails()
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




