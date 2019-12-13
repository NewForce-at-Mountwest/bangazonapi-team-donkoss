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
    public class ComputerControllerTest
    {
        // This is going to be our test Computer instance that we create and delete to make sure everything works
        private Computer dummyComputer { get; } = new Computer
        { 
            PurchaseDate = new DateTime (2018, 01, 01),
            DecomissionDate = new DateTime(2019, 01, 01),
            Make = "Dell",
            Manufacturer = "Windows",
            Archived = false
        };
        // We'll store our base url for this route as a private field to avoid typos
        private string url { get; } = "/api/computer";
        // Reusable method to create a new coffee in the database and return it
        public async Task<Computer> CreateDummyComputer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Serialize the C# object into a JSON string
                string DellAsJSON = JsonConvert.SerializeObject(dummyComputer);
                // Use the client to send the request and store the response
                HttpResponseMessage response = await client.PostAsync(
                    url,
                    new StringContent(DellAsJSON, Encoding.UTF8, "application/json")
                );
                // Store the JSON body of the response
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON into an instance of Computer
                Computer newlyCreatedComputer = JsonConvert.DeserializeObject<Computer>(responseBody);
                return newlyCreatedComputer;
            }
        }
        // Reusable method to delete a Computer from the database
        public async Task deleteDummyComputer(Computer computerToDelete)
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                HttpResponseMessage deleteResponse = await client.DeleteAsync($"{url}/{computerToDelete.Id}");
            }
        }
        /* TESTS START HERE */
        [Fact]
        public async Task Create_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Create a new Computer in the db
                Computer newDell = await CreateDummyComputer();
                // Try to get it again
                HttpResponseMessage response = await client.GetAsync($"{url}/{newDell.Id}");
                response.EnsureSuccessStatusCode();
                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                // Turn the JSON into C#
                Computer newComputer = JsonConvert.DeserializeObject<Computer>(responseBody);
                // Make sure it's really there
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(dummyComputer.PurchaseDate, newComputer.PurchaseDate);
                Assert.Equal(dummyComputer.DecomissionDate, newComputer.DecomissionDate);
                Assert.Equal(dummyComputer.Make, newComputer.Make);
                Assert.Equal(dummyComputer.Manufacturer, newComputer.Manufacturer);
                Assert.Equal(dummyComputer.Archived, newComputer.Archived);
                // Clean up after ourselves
                await deleteDummyComputer(newComputer);
            }
        }
        [Fact]
        public async Task Delete_Computer()
        {
            // Note: with many of these methods, I'm creating dummy data and then testing to see if I can delete it. I'd rather do that for now than delete something else I (or a user) created in the database, but it's not essential-- we could test deleting anything 
            // Create a new coffee in the db
            Computer newDell = await CreateDummyComputer();
            // Delete it
            await deleteDummyComputer(newDell);
            using (var client = new APIClientProvider().Client)
            {
                // Try to get it again
                HttpResponseMessage response = await client.GetAsync($"{url}{newDell.Id}");
                // Make sure it's really gone
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
        [Fact]
        public async Task Get_All_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Try to get all of the Computers from /api/Computer
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // Convert to JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                // Convert from JSON to C#
                List<Computer> computers = JsonConvert.DeserializeObject<List<Computer>>(responseBody);
                // Make sure we got back a 200 OK Status and that there are more than 0 Computers in our database
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computers.Count > 0);
            }
        }
        [Fact]
        public async Task Get_Single_Computer()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                // Create a dummy coffee
                Computer newDell = await CreateDummyComputer();
                // Try to get it
                HttpResponseMessage response = await client.GetAsync($"{url}/{newDell.Id}");
                response.EnsureSuccessStatusCode();
                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();
                // Turn the JSON into C#
                Computer DellFromDB = JsonConvert.DeserializeObject<Computer>(responseBody);
                // Did we get back what we expected to get back? 
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(dummyComputer.PurchaseDate, newDell.PurchaseDate);
                Assert.Equal(dummyComputer.DecomissionDate, newDell.DecomissionDate);
                Assert.Equal(dummyComputer.Make, newDell.Make);
                Assert.Equal(dummyComputer.Manufacturer, newDell.Manufacturer);
                Assert.Equal(dummyComputer.Archived, newDell.Archived);
                // Clean up after ourselves-- delete the dummy coffee we just created
                await deleteDummyComputer(DellFromDB);
            }
        }
        [Fact]
        public async Task Update_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Create a dummy Computer
                Computer newDell = await CreateDummyComputer();
                // Make a new Manufacturer and assign it to our dummy Computer
                string newManufacturer = "Dell";
                newDell.Manufacturer = newManufacturer;
                // Convert it to JSON
                string modifiedDellAsJSON = JsonConvert.SerializeObject(newDell);
                // Try to PUT the newly edited Computer
                var response = await client.PutAsync(
                        $"{url}/{newDell.Id}",
                        new StringContent(modifiedDellAsJSON, Encoding.UTF8, "application/json")
                    );
                // See what comes back from the PUT. Is it a 204? 
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                // Get the edited Computer back from the database after the PUT
                var getModifiedDell = await client.GetAsync($"{url}/{newDell.Id}");
                getModifiedDell.EnsureSuccessStatusCode();
                // Convert it to JSON
                string getComputerBody = await getModifiedDell.Content.ReadAsStringAsync();
                // Convert it from JSON to C#
                Computer newlyEditedComputer = JsonConvert.DeserializeObject<Computer>(getComputerBody);
                // Make sure the acctNumber was modified correctly
                Assert.Equal(HttpStatusCode.OK, getModifiedDell.StatusCode);
                Assert.Equal(newManufacturer, newlyEditedComputer.Manufacturer);
                // Clean up after yourself
                await deleteDummyComputer(newlyEditedComputer);
            }
        }
        [Fact]
        public async Task Test_Get_NonExitant_Computer_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Try to get a Computer with an Id that could never exist
                HttpResponseMessage response = await client.GetAsync($"{url}/00000000");
                // It should bring back a 204 no content error
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
        [Fact]
        public async Task Test_Delete_NonExistent_Computer_Fails()
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