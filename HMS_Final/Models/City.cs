namespace HMS_Final.Models
{
    public class City : BaseEntity<int>
    {
        public string Name {  get; set; }

        //Foreign Key
        public int CountryId { get; set; }

        //Navigator Property
        public Country Country { get; set; }

        //One-to-many
        public ICollection<Hospital> Hospitals { get; set; }
    }
}
