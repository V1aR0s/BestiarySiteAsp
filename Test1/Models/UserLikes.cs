using System.ComponentModel.DataAnnotations.Schema;

namespace Test1.Models
{
    public class UserLikes
    {

        public int Id { get; set; }


        [ForeignKey(nameof(ArticleId))]
        public СreatureArticle article { get; set; }
        public int ArticleId { get; set; }


        [ForeignKey(nameof(UserId))]
        public User user { get; set; }
        public string UserId { get; set; }
    }
}
