namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }

    }
    public interface IEntity
    {
        bool Deleted { get; set; }
    }
}
