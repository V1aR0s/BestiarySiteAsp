namespace Test1.Models
{
    public class Mythology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<СreatureArticle> сreatureArticles { get; set; }
        public string? ContentType { get; set; }
        public string? NameFile { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
