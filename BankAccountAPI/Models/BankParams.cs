namespace BankAccountAPI.Models
{
    public class BankParams
    {
        public string BankNumber { get; }
        public string AccountNumber { get; }
        public string UserId { get; }
        public string Pin { get; }
        public string HttpsEndpoint { get; }

        public string BankId {get;}

        public BankParams(string bankNumber, string accountNumber, string userId, string pin, string httpsEndpoint, string bankId)
        {
            BankNumber = bankNumber;
            AccountNumber = accountNumber;
            UserId = userId;
            Pin = pin;
            HttpsEndpoint = httpsEndpoint;
            BankId = bankId;
        }
    }
}