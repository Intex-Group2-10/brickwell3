namespace brickwell2.Models
{
	public interface ILegoRepository
	{
		public IQueryable<Customer> Customers { get; }
		public IQueryable<LineItem> LineItems { get; }
		public IQueryable<Order> Orders { get; }
		public IQueryable<Product> Products { get; }	
		public void AddOrder(Order order);
		public void EditOrder(Order order);
		public void DeleteOrder(Order order);
		
		public void AddCustomer(Customer customer);
		public void EditCustomer(Customer customer);
		public void DeleteCustomer(Customer customer);
		
		public void AddProduct(Product product);
		public void EditProduct(Product product);
		public void DeleteProduct(Product product);
		void SaveChanges();
	}
}
