namespace BankApp.Services
{
    public interface ICookieService
    {
        Task<int> GetUserId(CancellationToken cancellationToken);
    }
}
