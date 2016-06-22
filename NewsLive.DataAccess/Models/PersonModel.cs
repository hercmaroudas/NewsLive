namespace NewsLive.DataAccess.Models
{
    using System.Collections.Generic;

    public class PersonModel
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsPublisher { get; set; }

        public IEnumerable<Models.RoleModel> Roles { get; set; }
    }
}
