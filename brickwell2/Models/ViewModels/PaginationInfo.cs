namespace brickwell2.Models.ViewModels
{
    public class PaginationInfo
    {
        public int TotalProducts { get; set; }
        public int ProductsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalNumPages => (int) (Math.Ceiling((decimal)TotalProducts / ProductsPerPage));
    }
}