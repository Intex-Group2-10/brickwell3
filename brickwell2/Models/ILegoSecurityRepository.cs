namespace brickwell2.Models
{
    public interface ILegoSecurityRepository
    {
        IQueryable<AspNetUser> AspNetUsers { get; }
        public void EditUser(AspNetUser user);
        public void AddUser(AspNetUser user);
        public void DeleteUser(AspNetUser user);
    }
}