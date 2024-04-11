
namespace brickwell2.Models
{
	public class EFLegoRepository : ILegoRepository
	{
		private LegoDbContext _context;

		public EFLegoRepository(LegoDbContext temp)
		{
			_context = temp;
		}

		public IQueryable<Customer> Customers => _context.Customers;

		public IQueryable<LineItem> LineItems => _context.LineItems;

		public IQueryable<Order> Orders => _context.Orders;

		public IQueryable<Product> Products => _context.Products;
		
		public void AddOrder(Order order)
		{
			_context.Add(order);
			_context.SaveChanges();
		}
		public void EditOrder(Order order)
		{
			_context.Update(order);
			_context.SaveChanges();
		}
		public void DeleteOrder(Order order)
		{
			_context.Orders.Remove(order);
			_context.SaveChanges();
		}
		public void AddCustomer(Customer customer)
		{
			_context.Add(customer);
			_context.SaveChanges();
		}
		public void EditCustomer(Customer customer)
		{
			_context.Update(customer);
			_context.SaveChanges();
		}
		public void DeleteCustomer(Customer customer)
		{
			_context.Customers.Remove(customer);
			_context.SaveChanges();
		}
		
		public void AddProduct(Product product)
		{
			_context.Add(product);
			_context.SaveChanges();
		}
		
		public void EditProduct(Product product)
		{
			_context.Update(product);
			_context.SaveChanges();
		}
		public void DeleteProduct(Product product)
		{
			_context.Products.Remove(product);
			_context.SaveChanges();
		}
		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
