using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ExampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly string ConnectionString = "Host=localhost;Port=5432;Database=Store;User Id=postgres;Password=MarekAna4;";

        [HttpPost]
        public IActionResult Insert(Product product)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "INSERT INTO \"Product\" (\"StoreId\", \"Name\", \"Price\", \"ExpirationDate\", \"IsActive\", \"DateCreated\", \"DateUpdated\", \"CreatedByUserId\", \"UpdatedByUserId\") " +
                                 "VALUES (@StoreId, @Name, @Price, @ExpirationDate, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId)";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@StoreId", product.StoreId);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@ExpirationDate", product.ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", product.IsActive);
                        command.Parameters.AddWithValue("@DateCreated", product.DateCreated);
                        command.Parameters.AddWithValue("@DateUpdated", product.DateUpdated);
                        command.Parameters.AddWithValue("@CreatedByUserId", product.CreatedByUserId);
                        command.Parameters.AddWithValue("@UpdatedByUserId", product.UpdatedByUserId);

                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                return Ok("Product inserted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM \"Product\"";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    StoreId = reader.GetInt32(1),
                                    Name = reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    ExpirationDate = reader.GetDateTime(4).Date.ToDateOnly(),
                                    IsActive = reader.GetBoolean(5),
                                    DateCreated = reader.GetDateTime(6),
                                    DateUpdated = reader.GetDateTime(7),
                                    CreatedByUserId = reader.GetInt32(8),
                                    UpdatedByUserId = reader.GetInt32(9)
                                };

                                products.Add(product);
                            }
                        }
                        conn.Close();
                    }
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM \"Product\" WHERE \"Id\" = @Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    StoreId = reader.GetInt32(1),
                                    Name = reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    ExpirationDate = reader.GetDateTime(4).Date.ToDateOnly(),
                                    IsActive = reader.GetBoolean(5),
                                    DateCreated = reader.GetDateTime(6),
                                    DateUpdated = reader.GetDateTime(7),
                                    CreatedByUserId = reader.GetInt32(8),
                                    UpdatedByUserId = reader.GetInt32(9)
                                };

                                return Ok(product);
                            }
                        }
                    }
                    conn.Close();

                }

                return NotFound("Product not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "DELETE FROM \"Product\" WHERE \"Id\" = @Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound("Product not found.");

                        return Ok("Product deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
    }
}
