namespace NewsLive.DataAccess.Repository.Membership
{
    using System.Threading.Tasks;

    public interface IMembershipRepository
    {
        Models.MembershipModel Login(string userName, string password);

        Task<Models.MembershipModel> LoginAsync(string userName, string password);
    }
}
