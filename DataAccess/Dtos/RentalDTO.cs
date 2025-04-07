
using DataAccess.Models;

namespace DataAccess.DTOs
{
    public class RentalDTO
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        //public Car Car { get; set; } 
        //public string? CarLicensePlate { get; set; } 
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
       // public RentStatus Status { get; set; } 
    }
}
