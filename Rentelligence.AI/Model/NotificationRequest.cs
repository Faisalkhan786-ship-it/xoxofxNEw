namespace Rentelligence.AI.Model
{
    public class NotificationRequest
    {
        public List<string> Tokens { get; set; } = new();
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
    }
}
