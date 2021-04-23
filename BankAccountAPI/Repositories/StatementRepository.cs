using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace BankAccountAPI.Services
{
    public class StatementRepository : IStatementRepository
    {

        private readonly IMongoCollection<Statement> _statements;

        public StatementRepository(IStatementsDatabaseSettings statementsDatabaseSettings)
        {
            var client = new MongoClient(statementsDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(statementsDatabaseSettings.DatabaseName);
            _statements = database.GetCollection<Statement>(statementsDatabaseSettings.StatementsCollectionName);
        }
        public IEnumerable<Statement> GetStatements()
        {
            return _statements.Find(s => true).ToList();
        }

        public void StoreStatements(IEnumerable<Statement> statements)
        {
            _statements.InsertMany(statements);
        }
    }
}