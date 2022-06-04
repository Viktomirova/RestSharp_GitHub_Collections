using RestSharp;

namespace GitHub_Collections
{
    public class Issue : UserData
    {
        UserData user;
        RestClient client;

        public int id { get; set; }
        public string title { get; set; }
        public int number { get; set; }
        public string body { get; set; }
        public string html_url { get; set; }

    }
}
