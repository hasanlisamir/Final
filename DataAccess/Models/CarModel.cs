namespace DataAccess.Models
{
    public class CarModel : BaseModel
    {
        public string Name { get; set; } 
        public int Year { get; set; } 
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<Car> Cars { get; set; } 
    }
}
