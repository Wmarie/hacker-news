namespace HackerNewsApp.Shared

{
    public class Story
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public long Time { get; set; }
        public int Score { get; set; }
        public string? By { get; set; }
        public User? User { get; set; }
        public string? Type { get; set; }

    }
    public class User
    {
        public string Id { get; set; }
        public long Created { get; set; }
        public int Karma { get; set; }
    }
}
