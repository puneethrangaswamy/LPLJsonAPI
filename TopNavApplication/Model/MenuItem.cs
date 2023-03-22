namespace TopNavApplication.Model
{
    public class MenuItem
    {
        private int ID { get; set; }

        private string name { get; set; }

        private int typeID { get; set; }
        private int actionID { get; set; }
        private int loadConfigID { get; set; }

        private string url { get; set; }

        private string elementID { get; set; }

        private string elementClass { get; set; }

        private string additionalAttribute { get; set; }

        private bool isDeleted { get; set; }

    }
}
