namespace HMS_Final.Models
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
