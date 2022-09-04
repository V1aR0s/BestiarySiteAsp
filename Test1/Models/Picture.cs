using System.ComponentModel.DataAnnotations.Schema;

namespace Test1.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public string NameFile { get; set; }

        public string PhotoUrl { get; set; }


        [ForeignKey(nameof(CreaturArticleId))]
        public СreatureArticle CreatureArticle { get; set; }
        public int CreaturArticleId { get; set; }



    }
}
