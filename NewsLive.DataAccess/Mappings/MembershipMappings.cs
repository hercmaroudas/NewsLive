namespace NewsLive.DataAccess.Mappings
{
    using System.Linq;
    
    public static class MembershipMappings
    {
        public static Models.MembershipModel ToMembershipModel(this DataAccess.Membership entity)
        {
            return new Models.MembershipModel
                {
                    PersonId = entity.PersonId,
                    CreateOn = entity.CreateOn,
                    UserName = entity.UserName,
                    Person = new Models.PersonModel
                    {
                        PersonId = entity.PersonId,
                        FirstName = entity.Person.FirstName,
                        LastName = entity.Person.LastName,
                        Roles = entity.Person.Roles
                            .Select(r => new Models.RoleModel { RoleId = r.RoleId, Name = r.Name }),
                        IsPublisher = entity.Person.Roles
                            .Select(r => r.Name == "Publisher").FirstOrDefault()
                    }
                };
        }
    }
}
