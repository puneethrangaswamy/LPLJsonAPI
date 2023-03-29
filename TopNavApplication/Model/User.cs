namespace TopNavApplication.Model
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Address3 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Zipcode { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

    }
}
