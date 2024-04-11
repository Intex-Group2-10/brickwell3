
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
    }
}
