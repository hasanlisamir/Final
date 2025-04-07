namespace DataAccess.DTOs
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? AvailableFrom { get; set; }

        public int CarModelId { get; set; }
       
        
        

    
    }
}
