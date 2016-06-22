namespace NewsLive.DataAccess.Repository.Membership
{
    public interface IMembershipRepository
    {
        Models.MembershipModel Login(string userName, string password);
    }
}
