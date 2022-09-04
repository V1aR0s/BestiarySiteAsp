using System.ComponentModel.DataAnnotations.Schema;

namespace Test1.Models
{
    public class СreatureArticle
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }

        public string Etymology { get; set; }

        public string AuthorComment { get; set; }

        public string Literature { get; set; }

        public DateTime? dataCreate { get; set; }

        //пол -------------------------------------------------
        [ForeignKey(nameof(SexId))]
        public Sex Sex { get; set; }
        public int SexId { get; set; }

        //Опасность -------------------------------------------
        [ForeignKey(nameof(dangerTypeId))]
        public DangerType dangerType { get; set; }
        public int dangerTypeId { get; set; }

        //Мифология -------------------------------------------
        [ForeignKey(nameof(MythologyId))]
        public Mythology Mythology { get; set; }
        public int MythologyId { get; set; }

        //Тип -------------------------------------------------
        [ForeignKey(nameof(CreatureTypeId))]
        public CreatureType CreatureType { get; set; }
        public int CreatureTypeId { get; set; }


        [ForeignKey(nameof(articleStateId))]
        public ArticleState articleState { get; set; }
        public int articleStateId { get; set; }

        //Картинки -------------------------------------------
        public List<Picture> Pictures { get; set; }


    }
}
