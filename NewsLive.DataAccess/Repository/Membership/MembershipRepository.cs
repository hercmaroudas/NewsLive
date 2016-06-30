namespace NewsLive.DataAccess.Repository.Membership
{
    using System.Threading.Tasks;
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

        public async Task<Models.MembershipModel> LoginAsync(string userName, string password)
        {
            var membershipEntity = await _service.GetMembershipAsync(userName, string.IsNullOrEmpty(password) ? null : password);

            return membershipEntity == null ?
                null :
                membershipEntity.ToMembershipModel();
        }
    }
}
