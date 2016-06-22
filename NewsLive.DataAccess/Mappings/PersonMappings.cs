namespace NewsLive.DataAccess.Mappings
{
    public static class PersonMappings
    {
        public static Models.PersonModel ToPersonModel(this DataAccess.Person entity)
        {
            return new Models.PersonModel()
            {
                PersonId = entity.PersonId,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }
    }
}
