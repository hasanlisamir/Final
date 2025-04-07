namespace DataAccess.Models
{
    public class CarCategory : BaseModel
    {
        public string Name { get; set; } 
        public ICollection<CarModel> CarModels { get; set; } 
    }
}
