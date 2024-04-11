namespace brickwell2.Models
{
    public class EFLegoSecurityRepository : ILegoSecurityRepository
    {

        private LegoSecurityContext _context;

        public EFLegoSecurityRepository(LegoSecurityContext temp)
        {
            _context = temp;
        }

        public IQueryable<AspNetUser> AspNetUsers => _context.AspNetUsers;
        public void EditUser(AspNetUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
        public void AddUser(AspNetUser user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }
        public void DeleteUser(AspNetUser user)
        {
            _context.Remove(user);
            _context.SaveChanges();
        }
    }
}