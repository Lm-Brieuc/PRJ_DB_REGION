namespace PRJ_DB_REGION
{
    internal partial class Program
    {
        public class C_VILLE
        {
            public int id { get; set; }
            public string department_code { get; set; }
            public string insee_code { get; set; }
            public string zip_code { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public float gps_lat { get; set; }
            public float gps_lng { get; set; }
        }
    }
}
