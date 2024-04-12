namespace brickwell2.Models
{
	public interface ILegoRepository
	{
		public IQueryable<Customer> Customers { get; }
		public IQueryable<LineItem> LineItems { get; }
		public IQueryable<Order> Orders { get; }
		public IQueryable<Product> Products { get; }
		public IEnumerable<ItemBasedRecommendation> ItemBasedRecommendations { get; }
		
		public IEnumerable<UserBasedRecommendation> UserBasedRecommendations { get; }
		
		public void AddProduct(Product product);
		public void EditProduct(Product product);
		public void DeleteProduct(Product product);

		public IQueryable<UserCust> UserCusts { get; }
		void SaveChanges();
	}
}
