using Test1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Test1.ViewModels
{
    public class CreatureListViewModel
    {
        public IEnumerable<СreatureArticle> Articles { get; set; }
        public SelectList DangerType { get; set; }
        public SelectList Sex { get; set; }
        public SelectList articleState { get; set; }
        //articleState
        public SelectList CreatureType { get; set; }
        public SelectList Mythology { get; set; }
        public CreaturePageViewModel CreaturePageViewModel { get; set; }
        public FiltersViewModel filtersMain { get; set; }
    }
}