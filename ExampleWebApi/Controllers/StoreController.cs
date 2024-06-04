using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ExampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly string ConnectionString = "Host=localhost;Port=5432;Database=Store;User Id=postgres;Password=MarekAna4;";

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Store> stores = new List<Store>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM \"Store\"";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Store store = new Store
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Address = reader.GetString(2),
                                    PhoneNumber = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4),
                                    DateCreated = reader.GetDateTime(5),
                                    DateUpdated = reader.GetDateTime(6),
                                    CreatedByUserId = reader.GetInt32(7),
                                    UpdatedByUserId = reader.GetInt32(8)
                                };

                                stores.Add(store);
                            }
                        }
                    }
                    conn.Close();

                }

                return Ok(stores);
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

                    string sql = "SELECT * FROM \"Store\" WHERE \"Id\" = @Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Store store = new Store
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Address = reader.GetString(2),
                                    PhoneNumber = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4),
                                    DateCreated = reader.GetDateTime(5),
                                    DateUpdated = reader.GetDateTime(6),
                                    CreatedByUserId = reader.GetInt32(7),
                                    UpdatedByUserId = reader.GetInt32(8)
                                };

                                return Ok(store);
                            }
                        }
                    }
                    conn.Close();

                }

                return NotFound("Store not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Insert(Store store)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "INSERT INTO \"Store\" (\"Name\", \"Address\", \"PhoneNumber\", \"IsActive\", \"DateCreated\", \"DateUpdated\", \"CreatedByUserId\", \"UpdatedByUserId\") " +
                                 "VALUES (@Name, @Address, @PhoneNumber, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId)";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@Name", store.Name);
                        command.Parameters.AddWithValue("@Address", store.Address);
                        command.Parameters.AddWithValue("@PhoneNumber", store.PhoneNumber);
                        command.Parameters.AddWithValue("@IsActive", store.IsActive);
                        command.Parameters.AddWithValue("@DateCreated", store.DateCreated);
                        command.Parameters.AddWithValue("@DateUpdated", store.DateUpdated);
                        command.Parameters.AddWithValue("@CreatedByUserId", store.CreatedByUserId);
                        command.Parameters.AddWithValue("@UpdatedByUserId", store.UpdatedByUserId);

                        command.ExecuteNonQuery();
                    }
                    conn.Close();

                }

                return Ok("Store inserted successfully.");
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

                    string sql = "DELETE FROM \"Store\" WHERE \"Id\" = @Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound("Store not found.");

                        return Ok("Store deleted successfully.");
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
