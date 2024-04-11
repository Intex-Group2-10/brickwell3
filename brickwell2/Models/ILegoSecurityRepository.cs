namespace brickwell2.Models
{
    public interface ILegoSecurityRepository
    {
        IQueryable<AspNetUser> AspNetUsers { get; }
    }
}
