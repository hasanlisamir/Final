namespace DataAccess.Models
{
    public class Rental : BaseModel
    {
        public int CarId { get; set; }
        public Car Car { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; } 
        public RentStatus Status { get; set; }
    }
}
