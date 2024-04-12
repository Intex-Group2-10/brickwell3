namespace brickwell2.Models.ViewModels
{
    public class PaginationListViewModel
    {
        public IQueryable<Product>? Products { get; set; }
        public IQueryable<AspNetUser>? AspNetUsers { get; set; }
        public IQueryable<Order>? Orders { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo(); 
        public string? CurrentCategory { get; set; }
        public string? CurrentColor { get; set; }
    }
}