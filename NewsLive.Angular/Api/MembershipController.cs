namespace NewsLive.Angular.Api
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using DataAccess.Models;
    using DataAccess.Repository.Membership;
    
    public class MembershipController : ApiController
    {
        IMembershipRepository _repository;

        public MembershipController(IMembershipRepository repository)
        {
            _repository = repository;
        }

        // POST: api/Membership/Login/john/doe
        [HttpPost]
        public MembershipModel Login(LoginParameters parameter)
        {
            return _repository.Login(parameter.UserName, parameter.Password);
        }

        [HttpPost]
        public async Task<MembershipModel> LoginAsync(LoginParameters parameter)
        {
            return await _repository.LoginAsync(parameter.UserName, parameter.Password);
        }

        public class LoginParameters
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

    }
}
