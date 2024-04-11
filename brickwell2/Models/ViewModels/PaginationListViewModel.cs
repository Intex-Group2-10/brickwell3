namespace brickwell2.Models.ViewModels
{
    public class PaginationListViewModel
    {
        public IQueryable<Product>? Products { get; set; }
        public IQueryable<AspNetUser>? AspNetUsers { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo(); 

        public string? CurrentCategory { get; set; }
    }
}
