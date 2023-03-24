namespace TopNavApplication.Model
{
    public class User
    {
        public int id { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string country { get; set; }

        public string zipcode { get; set; }

        public string createdBy { get; set; }

        public DateTime createdAt { get; set; }

        public string modifiedBy { get; set; }

        public DateTime modifiedAt { get; set; }

    }
}
