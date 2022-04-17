namespace Core.Shared
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
