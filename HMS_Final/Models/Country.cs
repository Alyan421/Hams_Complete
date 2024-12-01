namespace HMS_Final.Models
{
    public class Country : BaseEntity<int>
    {
        public string CountryName {  get; set; }

        //one-to-many
        public ICollection<City> Cities { get; set; }

    }
}
