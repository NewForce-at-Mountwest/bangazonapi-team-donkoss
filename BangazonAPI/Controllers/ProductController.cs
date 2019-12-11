using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using BangazonAPI.Models;
namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ProductController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Product.Id, Product.Price, 
                                        Product.Title, Product.Description,
                                        Product.Quantity, Product.Archived,
                                        Product.CustomerId AS 'Customer Id',
                                        Product.ProductTypeId AS 'Product Type Id'
                                        FROM Product 
                                        LEFT JOIN Customer on Product.CustomerId = Customer.Id
                                        LEFT JOIN ProductType on Product.ProductTypeId = ProductType.Id";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Product> products = new List<Product>();
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("Product Type Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("Customer Id")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Archived = reader.GetBoolean(reader.GetOrdinal("Archived"))
                        };
                        products.Add(product);
                    }
                    reader.Close();
                    return Ok(products);
                }
            }
        }
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                                        @"SELECT Product.Id, Product.Price,
                                        Product.Title, Product.Description,
                                        Product.Quantity, Product.Archived,
                                        Product.CustomerId AS 'Customer Id',
                                        Product.ProductTypeId AS 'Product Type Id'
                                        FROM Product 
                                        LEFT JOIN Customer on Product.CustomerId = Customer.Id
                                        LEFT JOIN ProductType on Product.ProductTypeId = ProductType.Id
                                        WHERE Product.Id = @id";
                    
                    
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Product productType = null;
                    if (reader.Read())
                    {
                        productType = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("Product Type Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("Customer Id")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Archived = reader.GetBoolean(reader.GetOrdinal("Archived"))
                        };
                    }
                    reader.Close();
                    return Ok(productType);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product productType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, Archived)
                                        OUTPUT INSERTED.Id
                                        VALUES (@productTypeId, @customerId, @price, @title, @description, @quantity, @archived)";
                    cmd.Parameters.Add(new SqlParameter("@productTypeId", productType.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@customerId", productType.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@price", productType.Price));
                    cmd.Parameters.Add(new SqlParameter("@title", productType.Title));
                    cmd.Parameters.Add(new SqlParameter("@description", productType.Description));
                    cmd.Parameters.Add(new SqlParameter("@quantity", productType.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@archived", productType.Archived));
                    int newId = (int)cmd.ExecuteScalar();
                    productType.Id = newId;
                    return CreatedAtRoute("GetProduct", new { id = newId }, productType);
                }
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Product productType)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Product
                                            SET ProductTypeId = @productTypeId,
                                                CustomerId = @customerId,
                                                Price = @price,
                                                Title = @title,
                                                Description = @description,
                                                Quantity = @quantity,
                                                Archived = @archived
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@productTypeId", productType.ProductTypeId));
                        cmd.Parameters.Add(new SqlParameter("@customerId", productType.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@price", productType.Price));
                        cmd.Parameters.Add(new SqlParameter("@title", productType.Title));
                        cmd.Parameters.Add(new SqlParameter("@description", productType.Description));
                        cmd.Parameters.Add(new SqlParameter("@quantity", productType.Quantity));
                        cmd.Parameters.Add(new SqlParameter("@archived", productType.Archived));
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpDelete("{id}")]

        //public async Task<IActionResult> Delete([FromRoute] int id)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"UPDATE Product
        //                                    SET Archived = 1
        //                                    WHERE id = @id";
        //                cmd.Parameters.Add(new SqlParameter("@id", id));

        //                int rowsAffected = cmd.ExecuteNonQuery();
        //                if (rowsAffected > 0)
        //                {
        //                    return new StatusCodeResult(StatusCodes.Status204NoContent);
        //                }
        //                throw new Exception("No rows affected");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //Method for hard delete

        public async Task<IActionResult> Delete([FromRoute] int id, bool HardDelete)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        if (HardDelete == true) { 

                        cmd.CommandText = @"DELETE FROM Product WHERE Id = @id";
                    } else
                    {
                        cmd.CommandText = @"UPDATE Product
                                            SET Archived = 1
                                            WHERE id = @id";
                    }
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool ProductExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, ProductTypeId, CustomerId, Price, Title, Description, Quantity, Archived
                        FROM Product
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}