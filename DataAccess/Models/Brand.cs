namespace DataAccess.Models
{
    public class Brand : BaseModel
    {
        public string Name { get; set; }

        public ICollection<CarModel> CarModels { get; set; }
    }
}
