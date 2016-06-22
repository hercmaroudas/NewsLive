namespace NewsLive.DataAccess.Repository.Membership
{
    using NewsLive.DataAccess.Mappings;

    public class MembershipRepository : IMembershipRepository
    {
        IDataService _service;

        public MembershipRepository(IDataService service)
        {
            _service = service;
        }

        public Models.MembershipModel Login(string userName, string password)
        {
            var membershipEntity = _service.GetMembership(userName, string.IsNullOrEmpty(password) ? null : password);

            return membershipEntity == null ? 
                null :
                membershipEntity.ToMembershipModel();
        }
    }
}
