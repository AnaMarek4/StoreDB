using System.Text.Json.Serialization;

namespace ExampleWebApi
{
    public class Product
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
    }
}
