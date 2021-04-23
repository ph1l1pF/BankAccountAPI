namespace BankAccountAPI 
{
    public class StatementsDatabaseSettings : IStatementsDatabaseSettings
    {
        public string StatementsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IStatementsDatabaseSettings
    {
        string StatementsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}