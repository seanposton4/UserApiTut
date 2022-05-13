namespace UserApi.Models
{
    public class User
    {
        public int userId { get; set; }
        public string password { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string locations { get; set; } = string.Empty;

    }
}
