
namespace BankAccountAPI.Services
{
    public interface IStatementDownloadService
    {
         public void StartDownloadTimer(BankParams[] bankParams);
    }
}