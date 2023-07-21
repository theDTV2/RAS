namespace control.Manager.Interfaces
{
    public interface IDataBaseManager
    {
        Task DeleteOldLoginLinksAsync(CancellationToken StopToken);
        Task DeleteOldAccountsAsync(CancellationToken StopToken);
    }
}
