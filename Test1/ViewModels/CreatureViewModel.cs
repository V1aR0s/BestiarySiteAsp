using Test1.Models;

namespace Test1.ViewModels
{
    public class CreatureViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }

        public string Etymology { get; set; }

        public string AuthorComment { get; set; }

        public int articleStateId { get; set; }

        public string Literature { get; set; }

        //пол -------------------------------------------------
        public int SexId { get; set; }

        //Опасность -------------------------------------------
        public int dangerTypeId { get; set; }

        //Мифология -------------------------------------------
        public int MythologyId { get; set; }

        //Тип -------------------------------------------------
        public int CreatureTypeId { get; set; }

        //Картинки -------------------------------------------
        public IFormFile[]? Pictures { get; set; }




        public List<Picture>? PicturesDeleted { get; set; }
    }
}
