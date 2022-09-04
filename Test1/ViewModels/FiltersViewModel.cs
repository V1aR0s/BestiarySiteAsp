using Test1.Models;

namespace Test1.ViewModels
{
    public class FiltersViewModel
    {
        public List<Mythology>? mythologies { get; set; }
        public List<CreatureType>? creatureTypes { get; set; }
        public List<DangerType>? dangerTypes { get; set; }
        public List<Sex>? sexes { get; set; }
        public List<string> Cheked { get; set; }
        public FiltersViewModel()
        {
            mythologies = new List<Mythology>();
            creatureTypes = new List<CreatureType>();
            dangerTypes = new List<DangerType>();
            sexes = new List<Sex>();
            Cheked = new List<string>();
        }

    }
}
