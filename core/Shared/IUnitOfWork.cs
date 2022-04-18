namespace Core.Shared
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
