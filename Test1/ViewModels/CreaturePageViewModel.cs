namespace Test1.ViewModels
{
    public class CreaturePageViewModel
    {
        public int pageNumber { get; set; }
        public int pageCount { get; set; }
        public int ItemsPerPage { get; set; }


        public CreaturePageViewModel(int count, int itemsPerPage)
        {
            this.ItemsPerPage = itemsPerPage;
            this.pageCount = (int)Math.Ceiling((decimal)count / itemsPerPage);
        }

        public bool HasPrevPage => this.pageNumber > 1;
        public bool HasNextPage => this.pageNumber <= pageCount;
    }
}
