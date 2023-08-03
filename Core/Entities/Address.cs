namespace Core.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public virtual User user { get; set; }
        public string UserID { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        public string Country { get; set; }
        public int PostalCode { get; set; }
    }
}