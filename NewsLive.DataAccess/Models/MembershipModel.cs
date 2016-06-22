namespace NewsLive.DataAccess.Models
{
    using System;

    public class MembershipModel
    {
        public int PersonId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime? CreateOn { get; set; }

        public DateTime? LastLoginOn { get; set; }

        public Models.PersonModel Person { get; set; }
    }
}
