namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IAuditableEntity<TUserClass, TKey> : IEntity<TKey> where TUserClass : class
    {
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }

        TUserClass UserWhoCreated { get; set; }
        TUserClass UserWhoUpdated { get; set; }

    }
}
