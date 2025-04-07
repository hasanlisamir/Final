namespace DataAccess.Models
{
    public class Car : BaseModel
    {
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }

        public string LicensePlate { get; set; } 
        public string Color { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime? AvailableFrom { get; set; }
    }
}
